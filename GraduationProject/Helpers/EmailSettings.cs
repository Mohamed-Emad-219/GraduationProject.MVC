using System.Net;
using System.Net.Mail;
using GP.DAL.Models;
using GraduationProject.Appsettingsconfig;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using MailKit.Security;

namespace GraduationProject.Helpers
{
    public class EmailSettings
    {
        private readonly mailsettings _options;

        public EmailSettings(IOptions<mailsettings> options)
        {
            _options = options.Value;
        }

        public void SendEmail(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.Recipients));

            var builder = new BodyBuilder();
            // Set the HTML body here
            builder.HtmlBody = $@"
        <html>
        <body>
            <p style='font-size: 20px; font-weight: bold;'>Click the button below to reset your password:</p>
            <a href='{email.Body}' style='background-color: #4CAF50; color: white; padding: 5px 5px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; border-radius: 5px;'>Reset Password</a>
            <p style='font-size: 20px; font-weight: bold;'>If you did not request a password reset, please ignore this email.</p>
        </body>
        </html>";

            // You can still add a plain text body if you want for clients that don't support HTML
            builder.TextBody = "Please use this link to reset your password: " + email.Body;

            // Set the body of the email
            mail.Body = builder.ToMessageBody();
            mail.From.Add(MailboxAddress.Parse(_options.Email));
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }
}
