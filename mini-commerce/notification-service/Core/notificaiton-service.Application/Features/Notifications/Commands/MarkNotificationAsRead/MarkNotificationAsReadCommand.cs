using MediatR;

namespace notification_service.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommand : IRequest<bool>
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
    }
}
