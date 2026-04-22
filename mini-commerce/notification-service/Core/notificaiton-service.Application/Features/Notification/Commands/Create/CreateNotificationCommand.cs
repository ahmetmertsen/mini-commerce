using MediatR;
using notification_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Application.Features.Notification.Commands.Create
{
    public class CreateNotificationCommand : IRequest<CreateNotificationCommandResponse>
    {
        public Guid CustomerId { get; set; }
        public Guid? OrderId { get; set; }

        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }

        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;

        public bool SimulateSuccess { get; set; } = true;
    }
}
