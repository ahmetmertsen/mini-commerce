using customer_service.Domain.Entities;
using customer_service.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace customer_service.Persistence.Context
{
    public class CustomerServiceDbContext : DbContext
    {
        public CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<CustomerConsent> CustomerConsents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(customer => customer.Id);

                entity.Property(customer => customer.CustomerNumber)
                    .HasMaxLength(50);

                entity.Property(customer => customer.Type)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(customer => customer.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(customer => customer.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(customer => customer.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(customer => customer.Email)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(customer => customer.PhoneNumber)
                    .HasMaxLength(30);

                entity.HasIndex(customer => customer.AuthUserId)
                    .IsUnique()
                    .HasFilter("[AuthUserId] IS NOT NULL");

                entity.HasIndex(customer => customer.CustomerNumber)
                    .IsUnique()
                    .HasFilter("[CustomerNumber] IS NOT NULL");

                entity.HasIndex(customer => customer.Email);

                entity.HasMany(customer => customer.Addresses)
                    .WithOne(address => address.Customer)
                    .HasForeignKey(address => address.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(customer => customer.Consents)
                    .WithOne(consent => consent.Customer)
                    .HasForeignKey(consent => consent.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CustomerAddress>(entity =>
            {
                entity.HasKey(address => address.Id);

                entity.Property(address => address.Type)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(address => address.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(address => address.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(address => address.RecipientFullName)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(address => address.PhoneNumber)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(address => address.Country)
                    .HasMaxLength(100)
                    .HasDefaultValue("Turkey")
                    .IsRequired();

                entity.Property(address => address.City)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(address => address.District)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(address => address.AddressLine)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(address => address.PostalCode)
                    .HasMaxLength(20);

                entity.HasIndex(address => new { address.CustomerId, address.Status });

                entity.HasIndex(address => new { address.CustomerId, address.IsDefaultShipping })
                    .IsUnique()
                    .HasFilter("[IsDefaultShipping] = 1");

                entity.HasIndex(address => new { address.CustomerId, address.IsDefaultBilling })
                    .IsUnique()
                    .HasFilter("[IsDefaultBilling] = 1");
            });

            modelBuilder.Entity<CustomerConsent>(entity =>
            {
                entity.HasKey(consent => consent.Id);

                entity.Property(consent => consent.Type)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(consent => consent.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(consent => consent.Version)
                    .HasMaxLength(50);

                entity.Property(consent => consent.Source)
                    .HasMaxLength(100);

                entity.HasIndex(consent => new { consent.CustomerId, consent.Type, consent.Status });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
