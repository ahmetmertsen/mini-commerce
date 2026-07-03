using MediatR;

namespace notification_service.Application.Features.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommand : IRequest<bool>
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
    }
}
