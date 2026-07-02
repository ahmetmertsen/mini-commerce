using MediatR;
using Shared.Events.AuthEvents;

namespace notification_service.Application.Features.Notifications.Commands.ProcessNotificationRequested
{
    public class ProcessNotificationRequestedCommand : IRequest<ProcessNotificationRequestedCommandResponse>
    {
        public NotificationRequested Message { get; set; } = null!;
    }

    public class ProcessNotificationRequestedCommandResponse
    {
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public bool AlreadyProcessed { get; set; }
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}
