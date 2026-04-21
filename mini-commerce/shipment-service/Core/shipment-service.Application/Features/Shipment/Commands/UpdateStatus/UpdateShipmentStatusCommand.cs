using MediatR;
using shipment_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Commands.UpdateStatus
{
    public class UpdateShipmentStatusCommand : IRequest<UpdateShipmentStatusCommandResponse>
    {
        public Guid Id { get; set; }
        public ShipmentStatus Status { get; set; }
    }
}
