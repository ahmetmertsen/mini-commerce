using MediatR;

namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserRegisteredEvent
{
    public class ProcessAuthUserRegisteredEventCommand : IRequest<ProcessAuthUserRegisteredEventCommandResponse>
    {
        public Guid MessageId { get; set; }
        public Guid AuthUserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Guid CorrelationId { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
