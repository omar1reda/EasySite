using EasySite.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using MimeKit;
using System.Diagnostics.CodeAnalysis;
//using System.Net.Mail;

namespace EasySite.Helper.SendEmail
{
    public class MailSettings : IMailSettings
    {
        private readonly EmailSettings _options;

        public MailSettings(IOptions<EmailSettings> options)
        {
            this._options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var Mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,
            };

            Mail.To.Add(MailboxAddress.Parse(email.To));
            var bilder = new BodyBuilder();
            bilder.TextBody=email.Body; 
            Mail.Body = bilder.ToMessageBody();
            Mail.From.Add(new MailboxAddress(_options.DisplayName , _options.Email));

            using var smtp = new SmtpClient();

            smtp.Connect(_options.Host,_options.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email,_options.Password);
            smtp.Send(Mail);
            smtp.Disconnect(true);

        }

    }
}
