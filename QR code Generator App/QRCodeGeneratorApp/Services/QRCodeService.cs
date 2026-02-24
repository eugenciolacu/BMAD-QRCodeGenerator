using Net.Codecrete.QrCodeGenerator;
using System.Drawing;
using System.Drawing.Imaging;

namespace QRCodeGeneratorApp.Services
{
    /// <summary>
    /// Implementation of QR code generation service using the Net.Codecrete.QrCodeGenerator library.
    /// Generates SVG (as base64 data URI for preview) and image files (SVG/PNG for download) on demand.
    /// </summary>
    public class QRCodeService : IQRCodeService
    {
        /// <summary>
        /// Generates a QR code as a base64-encoded SVG data URI suitable for inline rendering in HTML img tags.
        /// </summary>
        /// <param name="text">QR code content (1-100 printable ASCII characters including letters, digits, space, and symbols).</param>
        /// <param name="ecc">Error correction level: L, M, Q, H.</param>
        /// <param name="version">QR code version (1-10).</param>
        /// <returns>Success flag with SVG data URI or error message.</returns>
        public (bool Success, string SvgOrError) GenerateSvg(string text, string ecc, int version)
        {
            // Defense-in-depth: enforce printable ASCII constraint at service layer (matches view model validation)
            if (string.IsNullOrEmpty(text) || !System.Text.RegularExpressions.Regex.IsMatch(text, @"^[\x20-\x7E]+$") || text.Length > 100)
                return (false, "Input must be 1–100 printable ASCII characters (letters, digits, spaces, and symbols such as @ ! # $ % & ' * + - . / : ; = ? ^ _ ` { | } ~).");

            try
            {
                var eccLevel = ecc switch
                {
                    "L" => QrCode.Ecc.Low,
                    "M" => QrCode.Ecc.Medium,
                    "Q" => QrCode.Ecc.Quartile,
                    "H" => QrCode.Ecc.High,
                    _ => throw new ArgumentException($"Invalid ECC level: {ecc}")
                };

                // Pin to exactly the requested version (minVersion = maxVersion = version).
                // EncodeText() always picks the minimum version; EncodeSegments() lets us fix it.
                // DataTooLongException is thrown (and caught below) if text doesn't fit.
                var segments = QrSegment.MakeSegments(text);
                var qr = QrCode.EncodeSegments(segments, eccLevel,
                    minVersion: version, maxVersion: version,
                    mask: -1, boostEcl: false);

                // Encode SVG as base64 data URI — renders cleanly in <img src=...>
                var svgString = qr.ToSvgString(4);
                var base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svgString));
                return (true, $"data:image/svg+xml;base64,{base64}");
            }
            catch (DataTooLongException)
            {
                return (false,
                    "The text is too long to encode with the selected Version and ECC level. " +
                    "Try a shorter text, a higher version, or a lower ECC level.");
            }
            catch (Exception ex)
            {
                return (false, $"QR code generation failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a QR code image as SVG or PNG bytes suitable for downloading and saving to disk.
        /// </summary>
        /// <param name="text">QR code content (1-100 printable ASCII characters including letters, digits, space, and symbols).</param>
        /// <param name="ecc">Error correction level: L, M, Q, H.</param>
        /// <param name="version">QR code version (1-10).</param>
        /// <param name="format">"svg" or "png" (case-insensitive); defaults to SVG.</param>
        /// <returns>Success flag, image bytes, content type, and error message on failure.</returns>
        public (bool Success, byte[] Data, string ContentType, string Error) GenerateImage(
            string text, string ecc, int version, string format)
        {
            // Defence-in-depth: same input validation as GenerateSvg (matches view model validation)
            if (string.IsNullOrEmpty(text) || !System.Text.RegularExpressions.Regex.IsMatch(text, @"^[\x20-\x7E]+$") || text.Length > 100)
                return (false, Array.Empty<byte>(), string.Empty, "Input must be 1–100 printable ASCII characters (letters, digits, spaces, and symbols such as @ ! # $ % & ' * + - . / : ; = ? ^ _ ` { | } ~).");

            // Normalise format — default to SVG
            bool isPng = string.Equals(format, "png", StringComparison.OrdinalIgnoreCase);

            try
            {
                var eccLevel = ecc switch
                {
                    "L" => QrCode.Ecc.Low,
                    "M" => QrCode.Ecc.Medium,
                    "Q" => QrCode.Ecc.Quartile,
                    "H" => QrCode.Ecc.High,
                    _ => throw new ArgumentException($"Invalid ECC level: {ecc}")
                };

                var segments = QrSegment.MakeSegments(text);
                var qr = QrCode.EncodeSegments(segments, eccLevel,
                    minVersion: version, maxVersion: version,
                    mask: -1, boostEcl: false);

                if (isPng)
                {
                    // Generate BMP bitmap from library then convert to PNG via System.Drawing (Windows)
                    const int scale = 10;
                    const int border = 4;
                    byte[] bmpBytes = qr.ToBmpBitmap(scale, border);

#pragma warning disable CA1416 // System.Drawing.Common is Windows-only; app targets Windows
                    using var bmpStream = new MemoryStream(bmpBytes);
                    using var bitmap = Image.FromStream(bmpStream);
                    using var pngStream = new MemoryStream();
                    bitmap.Save(pngStream, ImageFormat.Png);
#pragma warning restore CA1416
                    return (true, pngStream.ToArray(), "image/png", string.Empty);
                }
                else
                {
                    // SVG: return raw UTF-8 bytes (not base64 — for direct file download)
                    var svgString = qr.ToSvgString(4);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(svgString);
                    return (true, bytes, "image/svg+xml", string.Empty);
                }
            }
            catch (DataTooLongException)
            {
                return (false, Array.Empty<byte>(), string.Empty,
                    "The text is too long to encode with the selected Version and ECC level. " +
                    "Try a shorter text, a higher version, or a lower ECC level.");
            }
            catch (Exception ex)
            {
                return (false, Array.Empty<byte>(), string.Empty, $"QR code image generation failed: {ex.Message}");
            }
        }
    }
}
