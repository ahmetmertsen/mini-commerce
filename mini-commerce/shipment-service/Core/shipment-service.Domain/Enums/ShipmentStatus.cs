using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Domain.Enums
{
    public enum ShipmentStatus
    {
        Preparing,
        Shipped,
        Delivered,
        Cancelled
    }
}
