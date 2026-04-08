using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Features.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
    {
        public AddItemToCartCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Müşteri bilgisi boş olamaz.");

            RuleFor(x => x.CartItem)
                .NotNull()
                .WithMessage("Eklenecek ürün bilgisi boş olamaz.");

            RuleFor(x => x.CartItem.ProductId)
                .NotEmpty()
                .WithMessage("Ürün bilgisi boş olamaz.");

            RuleFor(x => x.CartItem.ProductVariantId)
                .NotEmpty()
                .WithMessage("Ürün varyantı boş olamaz.");

            RuleFor(x => x.CartItem.ProductName)
                .NotEmpty()
                .WithMessage("Ürün adı boş olamaz.");

            RuleFor(x => x.CartItem.UnitPrice)
                .GreaterThan(0)
                .WithMessage("Ürün fiyatı sıfırdan büyük olmalıdır.");

            RuleFor(x => x.CartItem.Quantity)
                .GreaterThan(0)
                .WithMessage("Ürün adedi sıfırdan büyük olmalıdır.");
        }
    }
}
