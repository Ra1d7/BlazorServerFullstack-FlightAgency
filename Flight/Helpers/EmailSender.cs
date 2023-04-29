using System.Net;
using System.Net.Mail;

namespace Flight.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private string smtpServer = string.Empty;
        private string smtpPort = string.Empty;
        private string smtpEmail = string.Empty;
        private string smtpPassword = string.Empty;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public record EmailData(string fullname, string email, string phonenumber, string message);
        public async Task<bool> SendEmailAsync(EmailData data)
        {
            smtpServer = _config.GetValue<string>("SmtpData:SmtpServer");
            smtpPort = _config.GetValue<string>("SmtpData:SmtpPort");
            smtpEmail = _config.GetValue<string>("SmtpData:Email");
            smtpPassword = _config.GetValue<string>("SmtpData:SmtpServer");
            try
            {

                var client = new SmtpClient("smtp.office365.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("your.email@live.com", "your password")
                };
                await client.SendMailAsync(
                    new MailMessage(from: "your.email@live.com",
                                    to: "your.email@live.com",
                                    $"Feedback from {data.fullname}",
                                    $"Message From: {data.fullname}\n Sender Email: {data.email}\n Sender Phonenumber: {data.phonenumber}\n\n\n {data.message}"
                                    ));
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
