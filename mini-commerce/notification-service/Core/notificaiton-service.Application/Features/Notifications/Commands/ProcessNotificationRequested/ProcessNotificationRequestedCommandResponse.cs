using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Application.Features.Notifications.Commands.ProcessNotificationRequested
{
    public class ProcessNotificationRequestedCommandResponse
    {
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public bool AlreadyProcessed { get; set; }
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}
