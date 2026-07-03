namespace notification_service.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string content);
        Task SendMailAsync(string[] toes, string subject, string content);
    }
}
