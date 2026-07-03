using MediatR;

namespace notification_service.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommand : IRequest<MarkAllNotificationsAsReadCommandResponse>
    {
        public Guid UserId { get; set; }
    }
}
