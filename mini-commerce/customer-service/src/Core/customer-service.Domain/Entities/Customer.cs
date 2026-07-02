using customer_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Domain.Entities
{
    public class Customer
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

        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
        public ICollection<CustomerConsent> Consents { get; set; } = new List<CustomerConsent>();

        private Customer() { }

        public static Customer CreateGuest(string firstName, string lastName, string email, string? phoneNumber = null)
        {
            return Create(null, CustomerType.Guest, firstName, lastName, email, phoneNumber);
        }

        public static Customer CreateRegistered(Guid authUserId, string firstName, string lastName, string email, string? phoneNumber = null)
        {
            if (authUserId == Guid.Empty)
            {
                throw new ArgumentException("Auth user id boş olamaz.", nameof(authUserId));
            }
                
            var customer = Create(authUserId, CustomerType.Registered, firstName, lastName, email, phoneNumber);
            customer.RegisteredAt = DateTime.UtcNow;

            return customer;
        }

        public void ConvertToRegistered(Guid authUserId)
        {
            if (authUserId == Guid.Empty)
            {
                throw new ArgumentException("Auth user id boş olamaz.", nameof(authUserId));
            }
                

            if (Type == CustomerType.Registered)
            {
                throw new InvalidOperationException("Müşteri zaten kayıtlı.");
            }
                
            AuthUserId = authUserId;
            Type = CustomerType.Registered;
            RegisteredAt = DateTime.UtcNow;
            ConvertedFromGuestAt = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }

        public void UpdateProfile(string firstName, string lastName, string email, string? phoneNumber = null)
        {
            EnsureRequired(firstName, nameof(firstName));
            EnsureRequired(lastName, nameof(lastName));
            EnsureRequired(email, nameof(email));

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim();
            UpdatedDate = DateTime.UtcNow;
        }

        public void AddAddress(CustomerAddress address)
        {
            if (address is null)
            {
                throw new ArgumentNullException(nameof(address));
            }
                
            address.CustomerId = Id;
            Addresses.Add(address);
            UpdatedDate = DateTime.UtcNow;
        }

        public void AddConsent(CustomerConsent consent)
        {
            if (consent is null)
            {
                throw new ArgumentNullException(nameof(consent));
            }
                
            consent.CustomerId = Id;
            Consents.Add(consent);
            UpdatedDate = DateTime.UtcNow;
        }

        private static Customer Create(Guid? authUserId, CustomerType type, string firstName, string lastName, string email, string? phoneNumber)
        {
            EnsureRequired(firstName, nameof(firstName));
            EnsureRequired(lastName, nameof(lastName));
            EnsureRequired(email, nameof(email));

            return new Customer
            {
                Id = Guid.NewGuid(),
                AuthUserId = authUserId,
                CustomerNumber = GenerateCustomerNumber(),
                Type = type,
                Status = CustomerStatus.Active,
                FirstName = firstName.Trim(),
                LastName = lastName.Trim(),
                Email = email.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim(),
                CreatedDate = DateTime.UtcNow
            };
        }

        private static string GenerateCustomerNumber()
        {
            return $"CUS-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}"[..28].ToUpperInvariant();
        }

        private static void EnsureRequired(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} boş olamaz.", parameterName);
            } 
        }
    }
}
