using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Domain.Enums
{
    public enum AuthOutboxStatus
    {
        Pending = 1,
        Processing,
        Processed,
        Failed
    }
}
