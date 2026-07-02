using Shared.Messages.Notification.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.AuthEvents
{
    public class NotificationRequested
    {
        public Guid NotificationId { get; set; } = Guid.NewGuid();
        public Guid? RecipientUserId { get; set; }
        public string? RecipientEmail { get; set; }
        public string? RecipientPhone { get; set; }
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public Dictionary<string, string> TemplateData { get; set; } = [];
        public bool IsSensitive { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
