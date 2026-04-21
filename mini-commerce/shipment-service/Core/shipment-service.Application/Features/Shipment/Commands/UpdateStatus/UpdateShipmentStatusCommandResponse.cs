using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Commands.UpdateStatus
{
    public class UpdateShipmentStatusCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public Guid ShipmentId { get; set; }
    }
}
