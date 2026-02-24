using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCodeGeneratorApp.Data;
using QRCodeGeneratorApp.Models;
using QRCodeGeneratorApp.Services;
using System.Security.Claims;

namespace QRCodeGeneratorApp.Controllers
{
    /// <summary>
    /// Handles QR code creation, browsing, filtering, details view, and download operations.
    /// All operations are restricted to authenticated users and scoped to their own QR codes.
    /// </summary>
    [Authorize]
    public class QRCodesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IQRCodeService _qrCodeService;
        private readonly IPdfExportService _pdfExportService;
        private readonly ILogger<QRCodesController> _logger;

        /// <summary>
        /// Initializes a new instance of the QRCodesController class.
        /// </summary>
        public QRCodesController(
            ApplicationDbContext context,
            IQRCodeService qrCodeService,
            IPdfExportService pdfExportService,
            ILogger<QRCodesController> logger)
        {
            _context = context;
            _qrCodeService = qrCodeService;
            _pdfExportService = pdfExportService;
            _logger = logger;
        }

        /// <summary>
        /// Displays the QR code creation form, optionally showing a preview of a previously saved QR code.
        /// </summary>
        /// <param name="previewId">Optional ID of a previously saved QR code to preview.</param>
        /// <returns>The Create view.</returns>
        // GET /QRCodes/Create
        [HttpGet]
        public IActionResult Create(int? previewId)
        {
            if (previewId.HasValue)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var saved = _context.QRCodes.Find(previewId.Value);
                if (saved != null && saved.UserId == userId)
                {
                    var (ok, svg) = _qrCodeService.GenerateSvg(
                        saved.DecodedText, saved.ErrorCorrectionLevel, saved.QRVersion);
                    if (ok)
                    {
                        ViewBag.QrSvg = svg;
                        ViewBag.QrFileName = saved.DecodedText;
                        ViewBag.SuccessMessage = "QR code saved successfully!";
                    }
                }
            }
            return View(new CreateQRCodeViewModel());
        }

        /// <summary>
        /// Creates and saves a new QR code.
        /// </summary>
        /// <param name="model">The QR code creation form data.</param>
        /// <returns>Redirects to Create action with the newly created QR code preview on success.</returns>
        // POST /QRCodes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQRCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, svgOrError) = _qrCodeService.GenerateSvg(
                model.DecodedText,
                model.ErrorCorrectionLevel,
                model.QRVersion);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, svgOrError);
                return View(model);
            }

            // UserId always from authenticated context — never from form input (AC: SEC-001)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var qrCode = new QRCode
            {
                UserId = userId,
                DecodedText = model.DecodedText,
                ErrorCorrectionLevel = model.ErrorCorrectionLevel,
                QRVersion = model.QRVersion,
                Notes = model.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.QRCodes.Add(qrCode);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "QR code created. UserId={UserId} QRCodeId={QRCodeId} ECC={ECC} Version={Version}",
                userId, qrCode.Id, qrCode.ErrorCorrectionLevel, qrCode.QRVersion);

            return RedirectToAction(nameof(Details), new { id = qrCode.Id, returnTo = "Create" });
        }

        /// <summary>
        /// Displays a paginated, filterable list of the authenticated user's QR codes.
        /// </summary>
        /// <param name="filterContent">Filter by QR code content text.</param>
        /// <param name="filterEcc">Filter by error correction level (L, M, Q, H).</param>
        /// <param name="filterVersion">Filter by QR code version.</param>
        /// <param name="filterNotes">Filter by notes text.</param>
        /// <param name="filterDateFrom">Filter by creation date (from).</param>
        /// <param name="filterDateTo">Filter by creation date (to).</param>
        /// <param name="sortBy">Sort by column: DecodedText, ErrorCorrectionLevel, QRVersion, CreatedAt, or Notes.</param>
        /// <param name="sortDir">Sort direction: asc or desc.</param>
        /// <param name="page">Page number for pagination (1-based).</param>
        /// <returns>The Index view with paginated and filtered QR code list.</returns>
        // GET /QRCodes — My QR Codes list (Story 3.2)
        [HttpGet]
        public async Task<IActionResult> Index(
            string? filterContent,
            string? filterEcc,
            int? filterVersion,
            string? filterNotes,
            DateTime? filterDateFrom,
            DateTime? filterDateTo,
            string? sortBy,
            string? sortDir,
            int page = 1)
        {
            const int pageSize = 10;
            const int truncateText = 40;
            const int truncateNotes = 60;

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            // --- Base query scoped to current user FIRST (SEC-001) ---
            var baseQuery = _context.QRCodes
                .Where(q => q.UserId == currentUserId);

            // Total count BEFORE filters — used to detect IsEmptyLibrary
            var totalLibraryCount = await baseQuery.CountAsync();

            // Apply per-column AND filters (server-side)
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(filterContent))
                filteredQuery = filteredQuery.Where(q => q.DecodedText.Contains(filterContent));
            if (!string.IsNullOrWhiteSpace(filterEcc))
                filteredQuery = filteredQuery.Where(q => q.ErrorCorrectionLevel.Contains(filterEcc));
            if (filterVersion.HasValue)
                filteredQuery = filteredQuery.Where(q => q.QRVersion == filterVersion.Value);
            if (!string.IsNullOrWhiteSpace(filterNotes))
                filteredQuery = filteredQuery.Where(q => q.Notes != null && q.Notes.Contains(filterNotes));
            if (filterDateFrom.HasValue)
            {
                var from = filterDateFrom.Value.Date;
                filteredQuery = filteredQuery.Where(q => q.CreatedAt >= from);
            }
            if (filterDateTo.HasValue)
            {
                var to = filterDateTo.Value.Date.AddDays(1);
                filteredQuery = filteredQuery.Where(q => q.CreatedAt < to);
            }

            // Count AFTER search — used for pagination metadata (SEC-003)
            var totalCount = await filteredQuery.CountAsync();
            int totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling((double)totalCount / pageSize);

            // Clamp page to valid range (TECH-001)
            page = Math.Max(1, Math.Min(page, totalPages));

            // Normalise sort params
            var validSortColumns = new[] { "DecodedText", "ErrorCorrectionLevel", "QRVersion", "CreatedAt", "Notes" };
            sortBy = validSortColumns.Contains(sortBy) ? sortBy : "CreatedAt";
            bool sortAsc = string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase);

            // Fetch paged data with dynamic sort
            var orderedQuery = sortBy switch
            {
                "DecodedText"          => sortAsc ? filteredQuery.OrderBy(q => q.DecodedText)          : filteredQuery.OrderByDescending(q => q.DecodedText),
                "ErrorCorrectionLevel" => sortAsc ? filteredQuery.OrderBy(q => q.ErrorCorrectionLevel) : filteredQuery.OrderByDescending(q => q.ErrorCorrectionLevel),
                "QRVersion"            => sortAsc ? filteredQuery.OrderBy(q => q.QRVersion)            : filteredQuery.OrderByDescending(q => q.QRVersion),
                "Notes"                => sortAsc ? filteredQuery.OrderBy(q => q.Notes)                : filteredQuery.OrderByDescending(q => q.Notes),
                _                      => sortAsc ? filteredQuery.OrderBy(q => q.CreatedAt)            : filteredQuery.OrderByDescending(q => q.CreatedAt),
            };

            var items = await orderedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to display ViewModel
            var viewItems = items.Select(q => new QRCodeListItemViewModel
            {
                Id = q.Id,
                DecodedText = q.DecodedText,
                DecodedTextDisplay = q.DecodedText.Length > truncateText
                    ? q.DecodedText[..truncateText] + "\u2026"
                    : q.DecodedText,
                ErrorCorrectionLevel = q.ErrorCorrectionLevel,
                QRVersion = q.QRVersion,
                CreatedAt = q.CreatedAt,
                NotesDisplay = q.Notes == null ? string.Empty
                    : q.Notes.Length > truncateNotes ? q.Notes[..truncateNotes] + "\u2026"
                    : q.Notes,
                Notes = q.Notes ?? string.Empty
            }).ToList();

            var vm = new QRCodesListViewModel
            {
                Items = viewItems,
                FilterContent = filterContent,
                FilterEcc = filterEcc,
                FilterVersion = filterVersion,
                FilterNotes = filterNotes,
                FilterDateFrom = filterDateFrom,
                FilterDateTo = filterDateTo,
                SortBy = sortBy,
                SortDir = sortAsc ? "asc" : "desc",
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                IsEmptyLibrary = totalLibraryCount == 0,
                NoSearchResults = totalLibraryCount > 0 && totalCount == 0
            };

            _logger.LogInformation(
                "QR code list accessed. UserId={UserId} FilterContent={FilterContent} FilterEcc={FilterEcc} FilterVersion={FilterVersion} FilterNotes={FilterNotes} FilterDateFrom={FilterDateFrom} FilterDateTo={FilterDateTo} Page={Page} ResultCount={ResultCount}",
                currentUserId, filterContent, filterEcc, filterVersion, filterNotes, filterDateFrom, filterDateTo, page, totalCount);

            return View(vm);
        }

        /// <summary>
        /// Displays detailed information about a specific QR code, including its preview.
        /// </summary>
        /// <param name="id">The ID of the QR code to display.</param>
        /// <returns>The Details view with QR code information, or NotFound if not owned by the user.</returns>
        // GET /QRCodes/Details/{id} — Story 3.3
        [HttpGet]
        public async Task<IActionResult> Details(int id, string? returnTo)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var qrCode = await _context.QRCodes.FindAsync(id);

            // Return NotFound for non-existent or other-user's QR codes to avoid revealing existence (Data Isolation)
            if (qrCode == null || qrCode.UserId != userId)
                return NotFound();

            var (success, svgOrError) = _qrCodeService.GenerateSvg(
                qrCode.DecodedText,
                qrCode.ErrorCorrectionLevel,
                qrCode.QRVersion);

            var vm = new QRCodeDetailsViewModel
            {
                Id = qrCode.Id,
                DecodedText = qrCode.DecodedText,
                ErrorCorrectionLevel = qrCode.ErrorCorrectionLevel,
                QRVersion = qrCode.QRVersion,
                CreatedAt = qrCode.CreatedAt,
                Notes = qrCode.Notes,
                PreviewDataUri = success ? svgOrError : string.Empty,
                ReturnTo = (returnTo == "Create") ? "Create" : "Index"
            };

            _logger.LogInformation(
                "QR code details viewed. UserId={UserId} QRCodeId={QRCodeId}",
                userId, id);

            return View(vm);
        }

        /// <summary>
        /// Generates and downloads a QR code as a PDF document.
        /// </summary>
        /// <param name="id">The ID of the QR code to download as PDF.</param>
        /// <returns>A PDF file if successful, or NotFound if not owned by the user.</returns>
        // GET /QRCodes/DownloadPdf/{id} — Story 4.1
        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            // Atomically verify ownership: both id AND userId must match (SEC-001)
            var qrCode = await _context.QRCodes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            // Return NotFound for non-existent or other-user's QR codes — no existence leak (AC: 7)
            if (qrCode == null)
            {
                _logger.LogWarning(
                    "DownloadPdf attempt failed. UserId={UserId} QRCodeId={QRCodeId} - not found or not owned",
                    userId, id);
                return NotFound();
            }

            // Generate QR code image as PNG for PDF embedding (AC: 3)
            var (success, imageBytes, _, error) = _qrCodeService.GenerateImage(
                qrCode.DecodedText,
                qrCode.ErrorCorrectionLevel,
                qrCode.QRVersion,
                "png");

            if (!success)
            {
                _logger.LogWarning(
                    "DownloadPdf image generation failed. UserId={UserId} QRCodeId={QRCodeId} Error={Error}",
                    userId, id, error);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to generate QR code image.");
            }

            // Generate PDF on demand — not cached or stored (AC: 2)
            var pdfBytes = _pdfExportService.GeneratePdf(qrCode, imageBytes);

            // Log successful download event (AUD-001)
            _logger.LogInformation(
                "QR code PDF downloaded. UserId={UserId} QRCodeId={QRCodeId}",
                userId, id);

            // Return file with meaningful filename and correct headers (AC: 6, 9)
            // File() sets Content-Type and Content-Disposition: attachment automatically
            return File(pdfBytes, "application/pdf", $"qrcode-{id}.pdf");
        }

        /// <summary>
        /// Generates and downloads a QR code as an image file (SVG or PNG format).
        /// </summary>
        /// <param name="id">The ID of the QR code to download.</param>
        /// <param name="format">Image format: "svg" (default) or "png".</param>
        /// <returns>An image file (SVG or PNG) if successful, or NotFound if not owned by the user.</returns>
        // GET /QRCodes/DownloadImage/{id}?format=svg|png — Story 4.2
        [HttpGet]
        public async Task<IActionResult> DownloadImage(int id, string format = "svg")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var qrCode = await _context.QRCodes.FindAsync(id);

            // Return NotFound for non-existent or other-user's QR codes (AC: 5)
            if (qrCode == null || qrCode.UserId != userId)
                return NotFound();

            // Enforce allowed formats — default to svg (AC: 2)
            if (!format.Equals("png", StringComparison.OrdinalIgnoreCase))
                format = "svg";

            var (success, data, contentType, error) = _qrCodeService.GenerateImage(
                qrCode.DecodedText,
                qrCode.ErrorCorrectionLevel,
                qrCode.QRVersion,
                format);

            if (!success)
            {
                _logger.LogWarning(
                    "DownloadImage generation failed. UserId={UserId} QRCodeId={QRCodeId} Format={Format} Error={Error}",
                    userId, id, format, error);
                return BadRequest(error);
            }

            // Meaningful filename: qrcode-{id}.svg or qrcode-{id}.png (AC: 4)
            var fileName = $"qrcode-{id}.{format.ToLower()}";

            _logger.LogInformation(
                "QR code image downloaded. UserId={UserId} QRCodeId={QRCodeId} Format={Format}",
                userId, id, format);

            // Content-Disposition set via FileResult (AC: 6)
            return File(data, contentType, fileName);
        }

        /// <summary>
        /// Deletes a QR code belonging to the authenticated user.
        /// </summary>
        /// <param name="id">The ID of the QR code to delete.</param>
        /// <returns>Redirects to Index on success, or NotFound if not owned by the user.</returns>
        // POST /QRCodes/Delete/{id} — Story 3.4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            // Verify ownership: query with both id AND userId atomically (SEC-001)
            var qrCode = await _context.QRCodes
                .FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

            // Return NotFound for non-existent or other-user's QR codes (AC: 3)
            if (qrCode == null)
            {
                _logger.LogWarning(
                    "Delete attempt failed. UserId={UserId} QRCodeId={QRCodeId} - not found or not owned",
                    userId, id);
                return NotFound();
            }

            // Store info for logging before deletion
            var qrCodeLabel = qrCode.DecodedText.Length > 50 
                ? qrCode.DecodedText[..50] + "..." 
                : qrCode.DecodedText;

            // Permanently remove from database (AC: 5)
            _context.QRCodes.Remove(qrCode);
            await _context.SaveChangesAsync();

            // Log deletion event (AC: 7)
            _logger.LogInformation(
                "QR code deleted. UserId={UserId} QRCodeId={QRCodeId} Label='{Label}'",
                userId, id, qrCodeLabel);

            // Redirect to My QR Codes list with success message (AC: 4)
            TempData["SuccessMessage"] = "QR code deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
