namespace QRCodeGeneratorApp.Services
{
    /// <summary>
    /// Service for generating QR code images in various formats (SVG or PNG).
    /// All generation is done on demand and not cached or stored.
    /// </summary>
    public interface IQRCodeService
    {
        /// <summary>
        /// Generates a QR code as a base64-encoded SVG data URI for inline preview rendering.
        /// </summary>
        /// <param name="text">QR code content text (1-100 ASCII alphanumeric characters).</param>
        /// <param name="ecc">Error correction level: L (Low), M (Medium), Q (Quartile), or H (High).</param>
        /// <param name="version">QR code version (1-10); higher versions encode more data.</param>
        /// <returns>A tuple with success flag and SVG data URI (on success) or error message (on failure).</returns>
        (bool Success, string SvgOrError) GenerateSvg(string text, string ecc, int version);

        /// <summary>
        /// Generates a QR code image as SVG or PNG bytes on demand (not persisted).
        /// </summary>
        /// <param name="text">QR code content.</param>
        /// <param name="ecc">Error correction level: L, M, Q, H.</param>
        /// <param name="version">QR version 1–10.</param>
        /// <param name="format">"svg" or "png" (case-insensitive); defaults to SVG.</param>
        /// <returns>A tuple with success flag, image bytes, content type, and error message.</returns>
        (bool Success, byte[] Data, string ContentType, string Error) GenerateImage(
            string text, string ecc, int version, string format);
    }
}
