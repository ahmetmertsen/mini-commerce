using notification_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? OrderId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string? FailureReason { get; set; }

        private Notification() { }

        public Notification(Guid customerId, Guid? orderId, NotificationType type, NotificationChannel channel, string title, string message)
        {
            if (customerId == Guid.Empty)
            {
                throw new ArgumentException("Geçerli bir müşteri bilgisi girilmelidir.");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Bildirim başlığı boş olamaz.");
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Bildirim mesajı boş olamaz.");
            }
            Id = Guid.NewGuid();
            CustomerId = customerId;
            OrderId = orderId;
            Type = type;
            Channel = channel;
            Title = title;
            Message = message;
            IsSent = false;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
