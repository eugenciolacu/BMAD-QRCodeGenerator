using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRCodeGeneratorApp.Data;
using QuestPDF.Infrastructure;
using Serilog;

using Microsoft.AspNetCore.RateLimiting;

// Register QuestPDF Community license (free for revenue < $1M).
// Must be set before any Document.Create() call at runtime (TECH-001).
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add rate limiting (basic fixed window, global policy)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10; // 10 requests
        limiterOptions.Window = TimeSpan.FromSeconds(10); // per 10 seconds
        limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 2;
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    {
        // Set to 'true' to require email confirmation before sign-in; set to 'false' to allow immediate login after registration.
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure password reset token lifespan (1 hour)
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromMinutes(60);
});

// Register EmailSender service
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, QRCodeGeneratorApp.Services.EmailSender>();

// Register QRCodeService
builder.Services.AddScoped<QRCodeGeneratorApp.Services.IQRCodeService, QRCodeGeneratorApp.Services.QRCodeService>();

// Register PdfExportService (Story 4.1)
builder.Services.AddScoped<QRCodeGeneratorApp.Services.IPdfExportService, QRCodeGeneratorApp.Services.PdfExportService>();

// Configure secure cookie settings for Identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    options.Cookie.Name = ".QRCodeGeneratorApp.Identity";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddRazorPages();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Auto-apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// Enable global anti-forgery token validation
app.Use(async (context, next) =>
{
    if (string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
    {
        await context.RequestServices.GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>()
        .ValidateRequestAsync(context);
    }
    await next();
});

app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
