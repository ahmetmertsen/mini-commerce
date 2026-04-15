using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using product_service.Application.Features.Product.Commands.AddVariant;
using product_service.Application.Features.Product.Commands.Create;
using product_service.Application.Features.Product.Commands.Update;
using product_service.Application.Features.Product.Commands.UpdateVariantStock;

namespace product_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public ProductController(IMediator mediatR)
        {
            _mediatR = mediatR;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("addProductVariant")]
        public async Task<IActionResult> AddProductVariant([FromBody] AddProductVariantCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Route("updateProductVariantStock")]
        public async Task<IActionResult> UpdateProductVariantStock([FromBody] UpdateProductVariantStockCommand request)
        {
            var response = await _mediatR.Send(request);
            return Ok(response);
        }



    }
}
