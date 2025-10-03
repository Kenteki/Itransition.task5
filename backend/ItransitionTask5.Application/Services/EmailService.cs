using System;
using System.Threading.Tasks;
using ItransitionTask5.Core.Abstractions;
using ItransitionTask5.Core.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ItransitionTask5.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _sendGridSettings;

        public EmailService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }
        public async Task SendVerificationEmailAsync(string toEmail, string userName, string verificationToken)
        {
            var client = new SendGridClient(_sendGridSettings.ApiKey);
            var from = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
            var to = new EmailAddress(toEmail, userName);

            var verificationUrl = $"{_sendGridSettings.VerificationUrl}?token={verificationToken}";

            // Message
            var subject = "Link to verify your Email";
            var htmlContent = $@"
                <html>
                <body>
                    <h1>Welcome to our team, {userName}!</h1>
                    <p>To verify your email use this <a href='{verificationUrl}' style='background-color: #4CAF50; color: white; padding: 14px 20px; text-decoration: none; display: inline-block;'>link to verify</a></p>
                    <p>Best regards,<br>Kenteki Team</p>
                </body>
                </html>
            ";

            // Plain text
            var plainTextContent = $@"
                Welcome to our team, {userName}!
                
                Welcome to our team, use the link above to verify your email.
                
                {verificationUrl}
                
                Best regards,
                Kenteki Team
            ";

            // Email message
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            try
            {
                var response = await client.SendEmailAsync(msg);

                // Logs
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    Console.WriteLine($"Failed to send email. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Logs
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}