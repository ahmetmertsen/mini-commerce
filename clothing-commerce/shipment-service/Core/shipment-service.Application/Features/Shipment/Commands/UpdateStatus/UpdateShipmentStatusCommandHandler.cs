using FluentValidation;
using MediatR;
using shipment_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Commands.UpdateStatus
{
    public class UpdateShipmentStatusCommandHandler : IRequestHandler<UpdateShipmentStatusCommand, UpdateShipmentStatusCommandResponse>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IValidator<UpdateShipmentStatusCommand> _validator;

        public UpdateShipmentStatusCommandHandler(IShipmentRepository shipmentRepository, IValidator<UpdateShipmentStatusCommand> validator)
        {
            _shipmentRepository = shipmentRepository;
            _validator = validator;
        }

        public async Task<UpdateShipmentStatusCommandResponse> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var shipment = await _shipmentRepository.GetByIdAsync(request.Id);
            if (shipment == null)
            {
                throw new Exception("Kargo kaydı bulunamadı.");
            }

            shipment.Status = request.Status;
            shipment.UpdatedDate = DateTime.UtcNow;

            await _shipmentRepository.SaveChangesAsync();

            return new UpdateShipmentStatusCommandResponse
            {
                Succeeded = true,
                Message = "Kargo durumu başarıyla güncellendi.",
                ShipmentId = shipment.Id
            };

        }
    }
}
