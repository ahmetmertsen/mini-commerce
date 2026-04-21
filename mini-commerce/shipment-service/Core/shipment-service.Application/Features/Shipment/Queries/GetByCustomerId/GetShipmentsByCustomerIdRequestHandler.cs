using FluentValidation;
using MediatR;
using shipment_service.Application.Dtos;
using shipment_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Queries.GetByCustomerId
{
    public class GetShipmentsByCustomerIdRequestHandler : IRequestHandler<GetShipmentsByCustomerIdRequest, List<ShipmentDto>>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IValidator<GetShipmentsByCustomerIdRequest> _validator;

        public GetShipmentsByCustomerIdRequestHandler(IShipmentRepository shipmentRepository, IValidator<GetShipmentsByCustomerIdRequest> validator)
        {
            _shipmentRepository = shipmentRepository;
            _validator = validator;
        }

        public async Task<List<ShipmentDto>> Handle(GetShipmentsByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var shipments = await _shipmentRepository.GetByCustomerIdAsync(request.CustomerId);

            return shipments.Select(shipment => new ShipmentDto
            {
                Id = shipment.Id,
                OrderId = shipment.OrderId,
                CustomerId = shipment.CustomerId,
                TrackingNumber = shipment.TrackingNumber,
                CarrierCompany = shipment.CarrierCompany,
                Status = shipment.Status,
                CreatedDate = shipment.CreatedDate
            }).ToList();
        }
    }
}
