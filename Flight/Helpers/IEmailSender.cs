using static Flight.Helpers.EmailSender;

namespace Flight.Helpers
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(EmailData data);
    }
}
