using customer_service.Application.Dtos;

namespace customer_service.Application.Features.Customer.Commands.CreateRegisteredCustomer
{
    public class CreateRegisteredCustomerCommandResponse
    {
        public bool Succeeded { get; set; }
        public bool AlreadyExists { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public string? CustomerNumber { get; set; }
        public CustomerDto? Customer { get; set; }
    }
}
