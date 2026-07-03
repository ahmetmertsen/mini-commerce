using customer_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Domain.Entities
{
    public class CustomerConsent
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public ConsentType Type { get; set; }
        public ConsentStatus Status { get; set; }

        public string? Version { get; set; }
        public string? Source { get; set; } // Checkout, Register, Profile, Admin
        public DateTime CreatedDate { get; set; }
        public DateTime? RevokedDate { get; set; }

        public Customer Customer { get; set; } = null!;

        private CustomerConsent() { }

        public static CustomerConsent Grant(Guid customerId, ConsentType type, string? version = null, string? source = null)
        {
            if (customerId == Guid.Empty)
                throw new ArgumentException("Customer id boş olamaz.", nameof(customerId));

            return new CustomerConsent
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Type = type,
                Status = ConsentStatus.Granted,
                Version = string.IsNullOrWhiteSpace(version) ? null : version.Trim(),
                Source = string.IsNullOrWhiteSpace(source) ? null : source.Trim(),
                CreatedDate = DateTime.UtcNow
            };
        }

        public void Revoke()
        {
            if (Status == ConsentStatus.Revoked)
                return;

            Status = ConsentStatus.Revoked;
            RevokedDate = DateTime.UtcNow;
        }
    }
}
