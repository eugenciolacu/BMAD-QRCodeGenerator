using QRCodeGeneratorApp.Models;

namespace QRCodeGeneratorApp.Services
{
    /// <summary>
    /// Generates a PDF document for a QR code on demand (not cached or stored).
    /// </summary>
    public interface IPdfExportService
    {
        /// <summary>
        /// Generates a PDF containing the QR code image and metadata.
        /// </summary>
        /// <param name="qrCode">The QR code entity supplying metadata fields.</param>
        /// <param name="qrImageBytes">PNG image bytes to embed in the PDF.</param>
        /// <returns>PDF content as a byte array.</returns>
        byte[] GeneratePdf(QRCode qrCode, byte[] qrImageBytes);
    }
}
