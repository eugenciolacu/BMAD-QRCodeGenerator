namespace QRCodeGeneratorApp.Models
{
    /// <summary>
    /// View model representing a single QR code item in a list view.
    /// Includes both full and truncated versions of text for display flexibility.
    /// </summary>
    public class QRCodeListItemViewModel
    {
        /// <summary>
        /// The primary key identifier for this QR code.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The full decoded text content (used as hidden data for row-selected action routing).
        /// </summary>
        public string DecodedText { get; set; } = string.Empty;

        /// <summary>
        /// Truncated version of DecodedText (max 40 chars + ellipsis) for table display.
        /// </summary>
        public string DecodedTextDisplay { get; set; } = string.Empty;

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
        /// Truncated notes (max 60 chars + ellipsis) for table display; empty string if notes are null.
        /// </summary>
        public string NotesDisplay { get; set; } = string.Empty;

        /// <summary>
        /// The full notes text for wrapping or expanded display.
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }
}
