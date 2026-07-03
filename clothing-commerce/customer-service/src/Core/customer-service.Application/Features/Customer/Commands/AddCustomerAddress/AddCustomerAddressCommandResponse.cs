using customer_service.Application.Dtos;

namespace customer_service.Application.Features.Customer.Commands.AddCustomerAddress
{
    public class AddCustomerAddressCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid AddressId { get; set; }
        public CustomerAddressDto? Address { get; set; }
    }
}
