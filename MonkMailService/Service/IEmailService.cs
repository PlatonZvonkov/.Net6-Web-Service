using MonkMailService.Models;

namespace MonkMailService.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailView request);
        Task<ICollection<MailBody>> GetAllMailAsync();
        Task LogMessageAsync(MailView mail);
    }
}
