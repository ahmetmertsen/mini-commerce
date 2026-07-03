using cart_service.Application.Dtos;
using cart_service.Application.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Features.Cart.Queries.GetCartByCustomerId
{
    public class GetCartByCustomerIdRequestHandler : IRequestHandler<GetCartByCustomerIdRequest, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<GetCartByCustomerIdRequest> _validator;

        public GetCartByCustomerIdRequestHandler(ICartRepository cartRepository, IValidator<GetCartByCustomerIdRequest> validator)
        {
            _cartRepository = cartRepository;
            _validator = validator;
        }

        public async Task<CartDto> Handle(GetCartByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var cart = await _cartRepository.GetByCustomerIdWithItemsAsync(request.CustomerId);
            if (cart == null)
            {
                throw new Exception("Sepet bulunamadı.");
            }

            CartDto cartDto = new()
            {
                CustomerId = cart.CustomerId,
                CartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.ProductName,
                    ProductVariantId = ci.ProductVariantId,
                    UnitPrice = ci.UnitPrice,
                    Quantity = ci.Quantity,
                    LineTotal = ci.LineTotal
                }).ToList(),
                TotalAmount = cart.TotalAmount,
                CreatedDate = cart.CreatedDate,
                UpdatedDate = cart.UpdatedDate
            };

            return cartDto;
        }
    }
}
