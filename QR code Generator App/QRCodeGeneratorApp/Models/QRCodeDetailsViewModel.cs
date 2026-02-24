namespace QRCodeGeneratorApp.Models
{
    /// <summary>
    /// View model for displaying detailed information about a specific QR code.
    /// </summary>
    public class QRCodeDetailsViewModel
    {
        /// <summary>
        /// The primary key of the QR code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The decoded/encoded text content of the QR code.
        /// </summary>
        public string DecodedText { get; set; } = string.Empty;

        /// <summary>
        /// The error correction level (L, M, Q, or H).
        /// </summary>
        public string ErrorCorrectionLevel { get; set; } = string.Empty;

        /// <summary>
        /// The QR code version (1-10).
        /// </summary>
        public int QRVersion { get; set; }

        /// <summary>
        /// The timestamp when the QR code was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Optional notes about this QR code.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Base64-encoded SVG data URI for the QR code preview image.
        /// </summary>
        public string PreviewDataUri { get; set; } = string.Empty;

        /// <summary>
        /// Indicates where the user should return to ("Create" or "Index"). Defaults to "Index".
        /// </summary>
        public string ReturnTo { get; set; } = "Index";
    }
}
