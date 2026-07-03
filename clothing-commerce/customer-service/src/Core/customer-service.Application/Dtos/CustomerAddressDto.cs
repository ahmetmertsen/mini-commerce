using customer_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Application.Dtos
{
    public class CustomerAddressDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public AddressType Type { get; set; }
        public AddressStatus Status { get; set; }
        public string Title { get; set; } = null!;
        public string RecipientFullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string? PostalCode { get; set; }
        public bool IsDefaultShipping { get; set; }
        public bool IsDefaultBilling { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
