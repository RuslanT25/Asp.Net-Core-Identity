
using Identity.Web.OptionModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Identity.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task ResetPassword(string resetPasswordLink, string To)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Host = _settings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_settings.Email, _settings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_settings.Email);
            mailMessage.To.Add(To);

            mailMessage.Subject = "Reset password link";
            mailMessage.Body = @$"
                    <h4>Click the link below for reset your password.</h4>
                    <p><a href='{resetPasswordLink}'>Click here</a></p>";
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
