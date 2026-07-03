using MassTransit;
using MediatR;
using notification_service.Application.Features.Notifications.Commands.ProcessNotificationRequested;
using Shared.Events.AuthEvents;

namespace notification_service.API.Consumers
{
    public class NotificationRequestedConsumer : IConsumer<NotificationRequested>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NotificationRequestedConsumer> _logger;

        public NotificationRequestedConsumer(IMediator mediator, ILogger<NotificationRequestedConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<NotificationRequested> context)
        {
            var response = await _mediator.Send(new ProcessNotificationRequestedCommand
            {
                Message = context.Message
            }, context.CancellationToken);

            if (response.AlreadyProcessed)
            {
                _logger.LogInformation(
                    "Notification message already processed. MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                    response.MessageId,
                    response.CorrelationId);
                return;
            }

            if (response.Succeeded)
            {
                _logger.LogInformation(
                    "Notification message consumed. MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                    response.MessageId,
                    response.CorrelationId);
                return;
            }

            _logger.LogWarning(
                "Notification message consume failed. MessageId: {MessageId}, CorrelationId: {CorrelationId}, Error: {Error}",
                response.MessageId,
                response.CorrelationId,
                response.Error);
        }
    }
}
