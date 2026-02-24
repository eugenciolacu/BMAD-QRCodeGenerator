using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCodeGeneratorApp.Models;

namespace QRCodeGeneratorApp.Services
{
    /// <inheritdoc />
    public class PdfExportService : IPdfExportService
    {
        /// <inheritdoc />
        public byte[] GeneratePdf(QRCode qrCode, byte[] qrImageBytes)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(style => style.FontFamily(Fonts.Arial).FontSize(11));

                    // ── Header ──────────────────────────────────────────────
                    page.Header()
                        .PaddingBottom(12)
                        .Text("QR Code Export")
                        .FontSize(22)
                        .Bold()
                        .FontColor(Colors.Grey.Darken3);

                    // ── Content ─────────────────────────────────────────────
                    page.Content()
                        .Column(col =>
                        {
                            col.Spacing(12);

                            // QR code image — centred, 15 cm wide
                            col.Item()
                                .AlignCenter()
                                .Width(15, Unit.Centimetre)
                                .Image(qrImageBytes)
                                .FitWidth();

                            // Horizontal rule
                            col.Item()
                                .PaddingVertical(4)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Lighten2);

                            // Metadata table
                            col.Item()
                                .Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1);
                                        columns.RelativeColumn(2);
                                    });

                                    // Row: Content
                                    table.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                                        .Text("Content").SemiBold();
                                    table.Cell().Padding(6)
                                        .Text(!string.IsNullOrEmpty(qrCode.DecodedText)
                                            ? qrCode.DecodedText : "N/A");

                                    // Row: ECC Level
                                    table.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                                        .Text("ECC Level").SemiBold();
                                    table.Cell().Padding(6)
                                        .Text(!string.IsNullOrEmpty(qrCode.ErrorCorrectionLevel)
                                            ? qrCode.ErrorCorrectionLevel : "N/A");

                                    // Row: QR Version
                                    table.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                                        .Text("QR Version").SemiBold();
                                    table.Cell().Padding(6)
                                        .Text(qrCode.QRVersion > 0
                                            ? qrCode.QRVersion.ToString() : "N/A");

                                    // Row: Created Date
                                    table.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                                        .Text("Created (UTC)").SemiBold();
                                    table.Cell().Padding(6)
                                        .Text(qrCode.CreatedAt != default
                                            ? qrCode.CreatedAt.ToString("yyyy-MM-dd HH:mm") : "N/A");
                                });
                        });

                    // ── Footer ──────────────────────────────────────────────
                    page.Footer()
                        .AlignCenter()
                        .Text(txt =>
                        {
                            txt.Span("Page ").FontColor(Colors.Grey.Medium);
                            txt.CurrentPageNumber().FontColor(Colors.Grey.Medium);
                            txt.Span(" of ").FontColor(Colors.Grey.Medium);
                            txt.TotalPages().FontColor(Colors.Grey.Medium);
                        });
                });
            }).GeneratePdf();
        }
    }
}
