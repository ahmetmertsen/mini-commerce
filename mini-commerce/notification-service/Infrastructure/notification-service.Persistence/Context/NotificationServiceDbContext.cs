using Microsoft.EntityFrameworkCore;
using notification_service.Domain.Entities;

namespace notification_service.Persistence.Context
{
    public class NotificationServiceDbContext : DbContext
    {
        public NotificationServiceDbContext(DbContextOptions<NotificationServiceDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationInbox> NotificationInboxes { get; set; }
        public DbSet<MailTemplate> MailTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(notification => notification.Id);
                entity.HasIndex(notification => notification.MessageId).IsUnique();
                entity.HasIndex(notification => notification.UserId);
                entity.HasIndex(notification => notification.CorrelationId);

                entity.Property(notification => notification.Type).HasConversion<string>().IsRequired();
                entity.Property(notification => notification.Channel).HasConversion<string>().IsRequired();
                entity.Property(notification => notification.Status).HasConversion<string>().IsRequired();
                entity.Property(notification => notification.RecipientEmail).HasMaxLength(320);
                entity.Property(notification => notification.RecipientPhone).HasMaxLength(32);
                entity.Property(notification => notification.Subject).HasMaxLength(250).IsRequired();
                entity.Property(notification => notification.Body).IsRequired();
                entity.Property(notification => notification.FailureReason).HasMaxLength(1000);
            });

            modelBuilder.Entity<NotificationInbox>(entity =>
            {
                entity.HasKey(inbox => inbox.Id);
                entity.HasIndex(inbox => inbox.MessageId).IsUnique();
                entity.HasIndex(inbox => inbox.CorrelationId);
                entity.HasIndex(inbox => inbox.Status);

                entity.Property(inbox => inbox.Type).HasConversion<string>().IsRequired();
                entity.Property(inbox => inbox.Channel).HasConversion<string>().IsRequired();
                entity.Property(inbox => inbox.Status).HasConversion<string>().IsRequired();
                entity.Property(inbox => inbox.Payload).IsRequired();
                entity.Property(inbox => inbox.Error).HasMaxLength(1000);
            });

            modelBuilder.Entity<MailTemplate>(entity =>
            {
                entity.HasKey(template => template.Id);
                entity.HasIndex(template => template.Name).IsUnique();
                entity.Property(template => template.Name).HasMaxLength(100).IsRequired();
                entity.Property(template => template.Value).IsRequired();
            });
        }
    }
}
