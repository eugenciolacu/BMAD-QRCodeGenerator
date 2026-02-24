using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QRCodeGeneratorApp.Data.Configurations;
using QRCodeGeneratorApp.Models;

namespace QRCodeGeneratorApp.Data
{
    /// <summary>
    /// The application's Entity Framework Core database context.
    /// Includes ASP.NET Core Identity tables and custom QR code domain data.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext class.
        /// </summary>
        /// <param name="options">Database context options configured in Program.cs.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        /// <summary>
        /// DbSet for accessing QR code records.
        /// </summary>
        public DbSet<QRCode> QRCodes { get; set; }

        /// <summary>
        /// Configures model relationships and constraints.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new QRCodeConfiguration());
        }
    }
}