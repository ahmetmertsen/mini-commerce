using MediatR;

namespace customer_service.Application.Features.Customer.Commands.CreateRegisteredCustomer
{
    public class CreateRegisteredCustomerCommand : IRequest<CreateRegisteredCustomerCommandResponse>
    {
        public Guid AuthUserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
