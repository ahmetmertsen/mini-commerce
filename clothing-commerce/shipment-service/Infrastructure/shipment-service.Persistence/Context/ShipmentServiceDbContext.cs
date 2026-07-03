using Microsoft.EntityFrameworkCore;
using shipment_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Persistence.Context
{
    public class ShipmentServiceDbContext : DbContext
    {
        public ShipmentServiceDbContext(DbContextOptions<ShipmentServiceDbContext> options) : base(options) { }

        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.Property(x => x.TrackingNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.CarrierCompany)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Status)
                    .HasConversion<string>();

                entity.HasIndex(x => x.TrackingNumber)
                    .IsUnique();

                entity.HasIndex(x => x.OrderId);
                entity.HasIndex(x => x.CustomerId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
