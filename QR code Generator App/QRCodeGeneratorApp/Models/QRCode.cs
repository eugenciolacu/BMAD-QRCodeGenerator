namespace QRCodeGeneratorApp.Models
{
    /// <summary>
    /// Represents a QR code entity stored in the database.
    /// </summary>
    public class QRCode
    {
        /// <summary>
        /// Primary key: unique identifier for this QR code record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key: ID of the user who owns this QR code.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// The decoded/encoded text content of the QR code.
        /// </summary>
        public string DecodedText { get; set; } = string.Empty;

        /// <summary>
        /// Error correction level (L, M, Q, or H).
        /// </summary>
        public string ErrorCorrectionLevel { get; set; } = string.Empty;

        /// <summary>
        /// QR code version (1-10); higher versions encode more data.
        /// </summary>
        public int QRVersion { get; set; }

        /// <summary>
        /// Timestamp when the QR code was created (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Optional user notes about this QR code.
        /// </summary>
        public string? Notes { get; set; }
    }
}
