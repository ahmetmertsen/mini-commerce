using invoice_service.Application.Features.Invoice.Commands.Create;
using invoice_service.Application.Features.Invoice.Queries.GetById;
using invoice_service.Application.Features.Invoice.Queries.GetByOrderId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace invoice_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public InvoiceController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediatR.Send(new GetInvoiceByIdRequest { Id = id });
            return Ok(response);
        }

        [HttpGet]
        [Route("getByOrderId/{orderId}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            var response = await _mediatR.Send(new GetInvoicesByOrderIdRequest { OrderId = orderId });
            return Ok(response);
        }
    }
}
