using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace QRCodeGeneratorApp.Services
{
    /// <summary>
    /// Email sending service using SMTP. Configuration is read from appsettings.json Email section.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromEmail;

        /// <summary>
        /// Initializes a new instance of the EmailSender class, loading SMTP configuration from appsettings.json.
        /// </summary>
        /// <param name="configuration">Application configuration provider.</param>
        /// <param name="logger">Logger instance for diagnostic and error logging.</param>
        /// <exception cref="InvalidOperationException">Thrown if SMTP configuration is incomplete.</exception>
        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _smtpServer = _configuration["Email:SmtpServer"] ?? throw new InvalidOperationException("Email:SmtpServer is not configured.");
            _smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            _smtpUser = _configuration["Email:SmtpUser"] ?? throw new InvalidOperationException("Email:SmtpUser is not configured.");
            _smtpPass = _configuration["Email:SmtpPass"] ?? throw new InvalidOperationException("Email:SmtpPass is not configured.");
            _fromEmail = _configuration["Email:From"] ?? throw new InvalidOperationException("Email:From is not configured.");

            _logger.LogInformation("EmailSender configured with SMTP server: {SmtpServer}:{SmtpPort}", _smtpServer, _smtpPort);
        }

        /// <summary>
        /// Sends an HTML email asynchronously via SMTP.
        /// </summary>
        /// <param name="email">Recipient email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="htmlMessage">Email body as HTML.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if SMTP communication fails.</exception>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation("Attempting to send email to {Email} from {FromEmail} via {SmtpServer}:{SmtpPort}", 
                email, _fromEmail, _smtpServer, _smtpPort);

            var message = new MailMessage();
            message.To.Add(email);
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = true;
            message.From = new MailAddress(_fromEmail);

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            _logger.LogInformation("SMTP Client configured - User: {SmtpUser}, SSL: {EnableSsl}, Port: {Port}", 
                _smtpUser, client.EnableSsl, _smtpPort);

            try
            {
                await client.SendMailAsync(message);
                _logger.LogInformation("Email successfully sent to {Email} with subject '{Subject}'", email, subject);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "SMTP error sending email to {Email}. Status: {StatusCode}, Message: {Message}", 
                    email, ex.StatusCode, ex.Message);
                throw new InvalidOperationException($"Failed to send email: {ex.Message}. Check Email configuration in appsettings.json", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error sending email to {Email}", email);
                throw;
            }
        }
    }
}
