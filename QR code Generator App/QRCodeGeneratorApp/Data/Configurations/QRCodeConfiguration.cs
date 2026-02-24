using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QRCodeGeneratorApp.Models;

namespace QRCodeGeneratorApp.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the QRCode model.
    /// Defines table schema, constraints, indexes, and relationships.
    /// </summary>
    public class QRCodeConfiguration : IEntityTypeConfiguration<QRCode>
    {
        /// <summary>
        /// Configures the QRCode entity mapping to the QRCodes table with all constraints and indexes.
        /// </summary>
        /// <param name="builder">The entity type builder for QRCode.</param>
        public void Configure(EntityTypeBuilder<QRCode> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Id)
                .ValueGeneratedOnAdd();

            builder.Property(q => q.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(q => q.DecodedText)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(q => q.ErrorCorrectionLevel)
                .IsRequired()
                .HasMaxLength(1);

            builder.Property(q => q.QRVersion)
                .IsRequired();

            builder.Property(q => q.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(q => q.Notes)
                .IsRequired(false)
                .HasMaxLength(300);

            // Foreign key relationship to AspNetUsers
            builder.HasOne<Microsoft.AspNetCore.Identity.IdentityUser>()
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Index on UserId for query performance
            builder.HasIndex(q => q.UserId)
                .HasDatabaseName("IX_QRCodes_UserId");

            builder.ToTable("QRCodes", t =>
            {
                t.HasCheckConstraint("CK_QRCodes_ErrorCorrectionLevel",
                    "[ErrorCorrectionLevel] IN ('L', 'M', 'Q', 'H')");
                t.HasCheckConstraint("CK_QRCodes_QRVersion",
                    "[QRVersion] >= 1 AND [QRVersion] <= 10");
            });
        }
    }
}
