using cart_service.Application.Features.Cart.Commands.AddItemToCart;
using cart_service.Application.Features.Cart.Queries.GetCartByCustomerId;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cart_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public CartController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Route("addItemToCart")]
        public async Task<IActionResult> AddItemToCart([FromBody] AddItemToCartCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("getCartByCustomerId/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(Guid customerId)
        {
            var response = await _mediatR.Send(new GetCartByCustomerIdRequest { CustomerId = customerId });
            return Ok(response);
        }

    }
}
