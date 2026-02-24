using System.ComponentModel.DataAnnotations;

namespace QRCodeGeneratorApp.Models
{
    /// <summary>
    /// View model for the QR code creation form. Properties are validated at both client and server side.
    /// </summary>
    public class CreateQRCodeViewModel
    {
        /// <summary>
        /// The text to be encoded in the QR code (1-100 printable ASCII characters including letters, digits, space, and symbols).
        /// </summary>
        [Required(ErrorMessage = "Input string is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Input string must be between 1 and 100 characters.")]
        [RegularExpression(@"^[\x20-\x7E]+$",
            ErrorMessage = "Printable ASCII characters only: letters, digits, spaces, and common symbols such as @ ! # $ % & ' * + - . / : ; = ? ^ _ ` ~ are allowed.")]
        [Display(Name = "Input String")]
        public string DecodedText { get; set; } = string.Empty;

        /// <summary>
        /// The error correction level (L, M, Q, or H). Defaults to M (Medium).
        /// </summary>
        [Required(ErrorMessage = "ECC Level is required.")]
        [RegularExpression(@"^(L|M|Q|H)$",
            ErrorMessage = "ECC Level must be L, M, Q, or H.")]
        [Display(Name = "Error Correction Level")]
        public string ErrorCorrectionLevel { get; set; } = "M";

        /// <summary>
        /// The QR code version (1-10). Defaults to 5 (Medium capacity).
        /// </summary>
        [Required(ErrorMessage = "QR Version is required.")]
        [Range(1, 10, ErrorMessage = "QR Version must be between 1 and 10.")]
        [Display(Name = "QR Version")]
        public int QRVersion { get; set; } = 5;

        /// <summary>
        /// Optional notes about this QR code (max 300 characters).
        /// </summary>
        [StringLength(300, ErrorMessage = "Notes cannot exceed 300 characters.")]
        public string? Notes { get; set; }

        // NO UserId field — always set from HttpContext.User in the controller
    }
}
