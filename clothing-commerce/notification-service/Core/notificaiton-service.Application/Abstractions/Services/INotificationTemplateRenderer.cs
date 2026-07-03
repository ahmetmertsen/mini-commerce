using Shared.Messages.Notification.Enums;

namespace notification_service.Application.Abstractions.Services
{
    public interface INotificationTemplateRenderer
    {
        string Render(string template, IReadOnlyDictionary<string, string> templateData);
        string ResolveTemplateName(NotificationType type);
        string ResolveSubject(NotificationType type);
    }
}
