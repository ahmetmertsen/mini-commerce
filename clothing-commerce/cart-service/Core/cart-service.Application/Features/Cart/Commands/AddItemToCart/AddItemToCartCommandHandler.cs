using cart_service.Application.Repositories;
using cart_service.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Features.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, AddItemToCartCommandResponse>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<AddItemToCartCommand> _validator;

        public AddItemToCartCommandHandler(ICartRepository cartRepository, IValidator<AddItemToCartCommand> validator)
        {
            _cartRepository = cartRepository;
            _validator = validator;
        }

        public async Task<AddItemToCartCommandResponse> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var cart = await _cartRepository.GetByCustomerIdWithItemsAsync(request.CustomerId);
            if (cart == null)
            {
                var cartItems = new List<CartItem>
                {
                    new CartItem(
                        request.CartItem.ProductId,
                        request.CartItem.ProductVariantId,
                        request.CartItem.ProductName,
                        request.CartItem.UnitPrice,
                        request.CartItem.Quantity)
                };

                cart = new Domain.Entities.Cart(request.CustomerId, cartItems);

                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();
                return new AddItemToCartCommandResponse
                {
                    Succeeded = true,
                    Message = "Ürün sepete eklendi.",
                    CartId = cart.Id
                };
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(x => x.ProductVariantId == request.CartItem.ProductVariantId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.CartItem.Quantity;
                existingItem.ProductName = request.CartItem.ProductName;
                existingItem.UnitPrice = request.CartItem.UnitPrice;
            }
            else
            {
                cart.CartItems.Add(new CartItem(
                    request.CartItem.ProductId,
                    request.CartItem.ProductVariantId,
                    request.CartItem.ProductName,
                    request.CartItem.UnitPrice,
                    request.CartItem.Quantity
                    ));
            }

            cart.TotalAmount = cart.CartItems.Sum(x => x.LineTotal);
            cart.UpdatedDate = DateTime.UtcNow;

            await _cartRepository.SaveChangesAsync();

            return new AddItemToCartCommandResponse
            {
                Succeeded = true,
                Message = "Ürün sepete eklendi.",
                CartId = cart.Id
            };

        }
    }
}
