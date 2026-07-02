using MediatR;

namespace customer_service.Application.Features.Customer.Commands.CreateGuestCustomer
{
    public class CreateGuestCustomerCommand : IRequest<CreateGuestCustomerCommandResponse>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
