using customer_service.Domain.Enums;

namespace customer_service.Application.Dtos
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public Guid? AuthUserId { get; set; }
        public string? CustomerNumber { get; set; }
        public CustomerType Type { get; set; }
        public CustomerStatus Status { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RegisteredAt { get; set; }
        public DateTime? ConvertedFromGuestAt { get; set; }
        public List<CustomerAddressDto> Addresses { get; set; } = new();
        public List<CustomerConsentDto> Consents { get; set; } = new();
    }

}
