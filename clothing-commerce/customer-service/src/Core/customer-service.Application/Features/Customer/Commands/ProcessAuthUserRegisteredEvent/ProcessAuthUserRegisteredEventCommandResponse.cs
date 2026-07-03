namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserRegisteredEvent
{
    public class ProcessAuthUserRegisteredEventCommandResponse
    {
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid? CustomerId { get; set; }
        public bool AlreadyProcessed { get; set; }
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}
