
using customer_service.Application.Features.Customer.Commands.ProcessAuthUserRegisteredEvent;
using MassTransit;
using MediatR;
using Shared.Events.AuthEvents;

namespace customer_service.API.Consumers
{
    public class AuthUserRegisteredEventConsumer : IConsumer<AuthUserRegisteredEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthUserRegisteredEventConsumer> _logger;

        public AuthUserRegisteredEventConsumer(IMediator mediator, ILogger<AuthUserRegisteredEventConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AuthUserRegisteredEvent> context)
        {
            var response = await _mediator.Send(new ProcessAuthUserRegisteredEventCommand
            {
                MessageId = context.Message.EventId,
                AuthUserId = context.Message.AuthUserId,
                FullName = context.Message.FullName,
                Email = context.Message.Email,
                CorrelationId = context.Message.CorrelationId,
                OccurredAt = context.Message.OccurredAt
            }, context.CancellationToken);

            if (response.AlreadyProcessed)
            {
                _logger.LogInformation(
                    "Auth User Registered message already processed. MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                    response.MessageId,
                    response.CorrelationId);
                return;
            }

            if (response.Succeeded)
            {
                _logger.LogInformation(
                    "Auth User Registered message consumed. MessageId: {MessageId}, CorrelationId: {CorrelationId}",
                    response.MessageId,
                    response.CorrelationId);
                return;
            }

            _logger.LogWarning(
                "Auth User Registered message consume failed. MessageId: {MessageId}, CorrelationId: {CorrelationId}, Error: {Error}",
                response.MessageId,
                response.CorrelationId,
                response.Error);

            throw new InvalidOperationException(response.Error ?? "Auth user registered event could not be processed.");
        }
    }
}
