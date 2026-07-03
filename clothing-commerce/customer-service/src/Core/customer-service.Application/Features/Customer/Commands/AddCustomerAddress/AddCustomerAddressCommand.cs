using customer_service.Domain.Enums;
using MediatR;

namespace customer_service.Application.Features.Customer.Commands.AddCustomerAddress
{
    public class AddCustomerAddressCommand : IRequest<AddCustomerAddressCommandResponse>
    {
        public Guid CustomerId { get; set; }
        public AddressType Type { get; set; }
        public string Title { get; set; } = null!;
        public string RecipientFullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = "Turkey";
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string? PostalCode { get; set; }
        public bool IsDefaultShipping { get; set; }
        public bool IsDefaultBilling { get; set; }
    }
}
