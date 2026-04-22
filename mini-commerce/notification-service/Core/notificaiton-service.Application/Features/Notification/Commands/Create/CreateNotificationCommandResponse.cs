using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Application.Features.Notification.Commands.Create
{
    public class CreateNotificationCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public Guid NotificationId { get; set; }
    }
}
