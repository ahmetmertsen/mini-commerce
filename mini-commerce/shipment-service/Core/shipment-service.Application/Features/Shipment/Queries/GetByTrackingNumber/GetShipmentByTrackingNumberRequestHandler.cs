using FluentValidation;
using MediatR;
using shipment_service.Application.Dtos;
using shipment_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Queries.GetByTrackingNumber
{
    public class GetShipmentByTrackingNumberRequestHandler : IRequestHandler<GetShipmentByTrackingNumberRequest, ShipmentDto>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IValidator<GetShipmentByTrackingNumberRequest> _validator;

        public GetShipmentByTrackingNumberRequestHandler(IShipmentRepository shipmentRepository, IValidator<GetShipmentByTrackingNumberRequest> validator)
        {
            _shipmentRepository = shipmentRepository;
            _validator = validator;
        }

        public async Task<ShipmentDto> Handle(GetShipmentByTrackingNumberRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            string fullTrackingNumber = $"TRK-{request.TrackingNumber}";
            var shipment = await _shipmentRepository.GetByTrackingNumberAsync(fullTrackingNumber);
            if (shipment == null)
            {
                throw new Exception("Takip numarasına ait kargo kaydı bulunamadı.");
            }

            return new ShipmentDto
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                CustomerId = shipment.CustomerId,
                TrackingNumber = shipment.TrackingNumber,
                CarrierCompany = shipment.CarrierCompany,
                Status = shipment.Status,
                CreatedDate = shipment.CreatedDate
            };
        }
    }
}
