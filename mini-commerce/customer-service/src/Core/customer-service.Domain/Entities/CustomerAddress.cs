using customer_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Domain.Entities
{
    public class CustomerAddress
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public AddressType Type { get; set; }
        public AddressStatus Status { get; set; }

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

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Customer Customer { get; set; } = null!;

        private CustomerAddress() { }

        public static CustomerAddress Create(Guid customerId, AddressType type, string title, string recipientFullName, string phoneNumber, string city, string district, string addressLine, string? postalCode = null, string country = "Turkey", bool isDefaultShipping = false, bool isDefaultBilling = false)
        {
            if (customerId == Guid.Empty)
            {
                throw new ArgumentException("Customer id boş olamaz.", nameof(customerId));
            }
                
            EnsureRequired(title, nameof(title));
            EnsureRequired(recipientFullName, nameof(recipientFullName));
            EnsureRequired(phoneNumber, nameof(phoneNumber));
            EnsureRequired(country, nameof(country));
            EnsureRequired(city, nameof(city));
            EnsureRequired(district, nameof(district));
            EnsureRequired(addressLine, nameof(addressLine));

            return new CustomerAddress
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Type = type,
                Status = AddressStatus.Active,
                Title = title.Trim(),
                RecipientFullName = recipientFullName.Trim(),
                PhoneNumber = phoneNumber.Trim(),
                Country = country.Trim(),
                City = city.Trim(),
                District = district.Trim(),
                AddressLine = addressLine.Trim(),
                PostalCode = string.IsNullOrWhiteSpace(postalCode) ? null : postalCode.Trim(),
                IsDefaultShipping = isDefaultShipping,
                IsDefaultBilling = isDefaultBilling,
                CreatedDate = DateTime.UtcNow
            };
        }

        public void MarkAsDefaultShipping()
        {
            IsDefaultShipping = true;
            UpdatedDate = DateTime.UtcNow;
        }

        public void MarkAsDefaultBilling()
        {
            IsDefaultBilling = true;
            UpdatedDate = DateTime.UtcNow;
        }

        public void Delete()
        {
            Status = AddressStatus.Deleted;
            IsDefaultShipping = false;
            IsDefaultBilling = false;
            UpdatedDate = DateTime.UtcNow;
        }

        private static void EnsureRequired(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{parameterName} boş olamaz.", parameterName);
        }
    }
}
