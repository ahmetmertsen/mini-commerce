using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notificaiton_service.Application.Features.Notification.Queries.GetByCustomerId;
using notificaiton_service.Application.Features.Notification.Queries.GetById;
using notification_service.Application.Features.Notification.Commands.Create;

namespace notification_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public NotificationController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateNotificationCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _mediatR.Send(new GetNotificationByIdRequest { Id = id });
            return Ok(response);
        }

        [HttpGet]
        [Route("getByCustomerId/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var response = await _mediatR.Send(new GetNotificationsByCustomerIdRequest { CustomerId = customerId });
            return Ok(response);
        }


    }
}
