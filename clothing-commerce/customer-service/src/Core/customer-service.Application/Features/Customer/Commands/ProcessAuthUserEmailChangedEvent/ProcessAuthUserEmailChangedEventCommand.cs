using MediatR;

namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserEmailChangedEvent
{
    public class ProcessAuthUserEmailChangedEventCommand : IRequest<ProcessAuthUserEmailChangedEventCommandResponse>
    {
        public Guid MessageId { get; set; }
        public Guid AuthUserId { get; set; }
        public string OldEmail { get; set; } = null!;
        public string NewEmail { get; set; } = null!;
        public Guid CorrelationId { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
