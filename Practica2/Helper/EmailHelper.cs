using MailKit.Net.Smtp;
using MimeKit;


namespace Practica2.Helper
{
    public class EmailHelper
    {
        private readonly IConfiguration _configuration;
        public EmailHelper(IConfiguration configuration)
        {
           
            _configuration = configuration;
        }
        public void SendEmail(string Email)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_configuration["SMTP:Email"]));
            mail.To.Add(MailboxAddress.Parse(Email));
            mail.Subject = "Welcome to FIFA";
            mail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "Thank you for your registration." };
            using var smtp = new SmtpClient();
            smtp.CheckCertificateRevocation=false;
            smtp.Connect(_configuration["SMTP:Servidor"], int.Parse(_configuration["SMTP:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["SMTP:Email"], _configuration["SMTP:Password"]);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }
    }
}
