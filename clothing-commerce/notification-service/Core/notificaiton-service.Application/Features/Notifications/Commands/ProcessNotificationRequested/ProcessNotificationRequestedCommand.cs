using MediatR;
using Shared.Events.AuthEvents;

namespace notification_service.Application.Features.Notifications.Commands.ProcessNotificationRequested
{
    public class ProcessNotificationRequestedCommand : IRequest<ProcessNotificationRequestedCommandResponse>
    {
        public NotificationRequested Message { get; set; } = null!;
    }
}
