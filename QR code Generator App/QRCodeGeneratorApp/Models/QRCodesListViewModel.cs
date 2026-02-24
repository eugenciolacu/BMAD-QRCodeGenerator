namespace QRCodeGeneratorApp.Models
{
    /// <summary>
    /// View model for the QR code list/index view with filtering, sorting, and pagination support.
    /// </summary>
    public class QRCodesListViewModel
    {
        /// <summary>
        /// The items to display on the current page.
        /// </summary>
        public List<QRCodeListItemViewModel> Items { get; set; } = new();

        /// <summary>
        /// Filter by QR code content text.
        /// </summary>
        public string? FilterContent { get; set; }

        /// <summary>
        /// Filter by error correction level (L, M, Q, or H).
        /// </summary>
        public string? FilterEcc { get; set; }

        /// <summary>
        /// Filter by QR code version.
        /// </summary>
        public int? FilterVersion { get; set; }

        /// <summary>
        /// Filter by notes text.
        /// </summary>
        public string? FilterNotes { get; set; }

        /// <summary>
        /// Filter by creation date (from, inclusive).
        /// </summary>
        public DateTime? FilterDateFrom { get; set; }

        /// <summary>
        /// Filter by creation date (to, inclusive).
        /// </summary>
        public DateTime? FilterDateTo { get; set; }

        /// <summary>
        /// Indicates whether any active filters are currently applied.
        /// </summary>
        public bool HasActiveFilters =>
            !string.IsNullOrWhiteSpace(FilterContent) ||
            !string.IsNullOrWhiteSpace(FilterEcc) ||
            FilterVersion.HasValue ||
            !string.IsNullOrWhiteSpace(FilterNotes) ||
            FilterDateFrom.HasValue ||
            FilterDateTo.HasValue;

        /// <summary>
        /// Current page number (1-based).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Column to sort by: DecodedText, ErrorCorrectionLevel, QRVersion, CreatedAt, or Notes.
        /// </summary>
        public string SortBy { get; set; } = "CreatedAt";

        /// <summary>
        /// Sort direction: "asc" (ascending) or "desc" (descending).
        /// </summary>
        public string SortDir { get; set; } = "desc";

        /// <summary>
        /// Total count of items matching the filters (after search, before pagination).
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages based on TotalCount and PageSize.
        /// </summary>
        public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Indicates whether the user has no QR codes at all (library is completely empty).
        /// </summary>
        public bool IsEmptyLibrary { get; set; }

        /// <summary>
        /// Indicates whether the user has QR codes but the current search/filter returned no results.
        /// </summary>
        public bool NoSearchResults { get; set; }
    }
}
