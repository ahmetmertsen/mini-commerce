using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Domain.Enums
{
    public enum NotificationStatus
    {
        Pending = 1,
        Sent = 2,
        Failed = 3,
        Cancelled = 4
    }
}
