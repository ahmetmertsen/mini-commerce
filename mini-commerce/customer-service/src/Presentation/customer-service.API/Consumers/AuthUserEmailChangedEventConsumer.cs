using customer_service.Application.Features.Customer.Commands.ProcessAuthUserEmailChangedEvent;
using MassTransit;
using MediatR;
using Shared.Events.AuthEvents;

namespace customer_service.API.Consumers
{
    public class AuthUserEmailChangedEventConsumer : IConsumer<AuthUserEmailChangedEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthUserEmailChangedEventConsumer> _logger;

        public AuthUserEmailChangedEventConsumer(IMediator mediator, ILogger<AuthUserEmailChangedEventConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AuthUserEmailChangedEvent> context)
        {
            var response = await _mediator.Send(new ProcessAuthUserEmailChangedEventCommand
            {
                MessageId = context.Message.EventId,
                AuthUserId = context.Message.AuthUserId,
                OldEmail = context.Message.OldEmail,
                NewEmail = context.Message.NewEmail,
                CorrelationId = context.Message.CorrelationId,
                OccurredAt = context.Message.OccurredAt
            }, context.CancellationToken);

            if (response.AlreadyProcessed)
            {
                _logger.LogInformation(
                    "Auth User Email Changed message already processed. MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                    response.MessageId,
                    response.CorrelationId);
                return;
            }

            if (response.Succeeded)
            {
                _logger.LogInformation(
                    "Auth User Email Changed message consumed. MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                    response.MessageId,
                    response.CorrelationId);
                return;
            }

            _logger.LogWarning(
                "Auth User Email Changed message consume failed. MessageId: {MessageId}, CorrelationId: {CorrelationId}, Error: {Error}",
                response.MessageId,
                response.CorrelationId,
                response.Error);

            throw new InvalidOperationException(response.Error ?? "Auth user email changed event could not be processed.");
        }
    }
}
