using Microsoft.EntityFrameworkCore;
using notification_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Persistence.Context
{
    public class NotificationServiceDbContext : DbContext
    {
        public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(x => x.Type)
                    .HasConversion<string>();

                entity.Property(x => x.Channel)
                    .HasConversion<string>();

                entity.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Message)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(x => x.FailureReason)
                    .HasMaxLength(500);

                entity.HasIndex(x => x.CustomerId);
                entity.HasIndex(x => x.OrderId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
