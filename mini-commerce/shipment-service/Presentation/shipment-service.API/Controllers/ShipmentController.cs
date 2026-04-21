using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shipment_service.Application.Features.Shipment.Commands.Create;
using shipment_service.Application.Features.Shipment.Commands.UpdateStatus;
using shipment_service.Application.Features.Shipment.Queries.GetByCustomerId;
using shipment_service.Application.Features.Shipment.Queries.GetById;
using shipment_service.Application.Features.Shipment.Queries.GetByOrderId;
using shipment_service.Application.Features.Shipment.Queries.GetByTrackingNumber;

namespace shipment_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public ShipmentController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediatR.Send(new GetShipmentByIdRequest { Id = id });
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateShipmentCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getByOrderId/{orderId}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            var response = await _mediatR.Send(new GetShipmentsByOrderIdRequest { OrderId = orderId });
            return Ok(response);
        }

        [HttpPut]
        [Route("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateShipmentStatusCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getByCustomerId/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var response = await _mediatR.Send(new GetShipmentsByCustomerIdRequest { CustomerId = customerId });
            return Ok(response);
        }

        [HttpGet]
        [Route("getByTrackingNumber/{trackingNumber}")]
        public async Task<IActionResult> GetByTrackingNumber(string trackingNumber)
        {
            var response = await _mediatR.Send(new GetShipmentByTrackingNumberRequest { TrackingNumber = trackingNumber });
            return Ok(response);
        }


    }
}
