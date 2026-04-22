using notification_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Dtos
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? OrderId { get; set; }
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SentDate { get; set; }
        public string? FailureReason { get; set; }
    }
}
