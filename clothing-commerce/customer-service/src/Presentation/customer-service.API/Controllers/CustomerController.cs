using customer_service.Application.Features.Customer.Commands.AddCustomerAddress;
using customer_service.Application.Features.Customer.Commands.CreateGuestCustomer;
using customer_service.Application.Features.Customer.Commands.CreateRegisteredCustomer;
using customer_service.Application.Features.Customer.Queries.GetCustomerByAuthUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace customer_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("createGuest")]
        public async Task<IActionResult> CreateGuest([FromBody] CreateGuestCustomerCommand request)
        {
            _logger.LogInformation("Create guest customer endpoint called.");

            var response = await _mediator.Send(request);

            _logger.LogInformation(
                "Create guest customer endpoint completed. CustomerId: {CustomerId}, Succeeded: {Succeeded}",
                response.CustomerId,
                response.Succeeded);

            return Ok(response);
        }

        [HttpPost]
        [Route("createRegistered")]
        public async Task<IActionResult> CreateRegistered([FromBody] CreateRegisteredCustomerCommand request)
        {
            _logger.LogInformation("Create registered customer endpoint called. AuthUserId: {AuthUserId}", request.AuthUserId);

            var response = await _mediator.Send(request);

            _logger.LogInformation(
                "Create registered customer endpoint completed. AuthUserId: {AuthUserId}, CustomerId: {CustomerId}, AlreadyExists: {AlreadyExists}, Succeeded: {Succeeded}",
                request.AuthUserId,
                response.CustomerId,
                response.AlreadyExists,
                response.Succeeded);

            return Ok(response);
        }

        [HttpGet]
        [Route("resolveByAuthUserId/{authUserId}")]
        public async Task<IActionResult> ResolveByAuthUserId(Guid authUserId)
        {
            _logger.LogInformation("Resolve customer by auth user id endpoint called. AuthUserId: {AuthUserId}", authUserId);

            var response = await _mediator.Send(new GetCustomerByAuthUserIdQuery { AuthUserId = authUserId });

            _logger.LogInformation(
                "Resolve customer by auth user id endpoint completed. AuthUserId: {AuthUserId}, CustomerId: {CustomerId}",
                authUserId,
                response.Id);

            return Ok(response);
        }

        [HttpPost]
        [Route("{customerId}/addresses")]
        public async Task<IActionResult> AddAddress(Guid customerId, [FromBody] AddCustomerAddressCommand request)
        {
            request.CustomerId = customerId;

            _logger.LogInformation("Add customer address endpoint called. CustomerId: {CustomerId}", customerId);

            var response = await _mediator.Send(request);

            _logger.LogInformation(
                "Add customer address endpoint completed. CustomerId: {CustomerId}, AddressId: {AddressId}, Succeeded: {Succeeded}",
                customerId,
                response.AddressId,
                response.Succeeded);

            return Ok(response);
        }
    }
}
