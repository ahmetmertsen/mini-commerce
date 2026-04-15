using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.AddVariant
{
    public class AddProductVariantCommandValidator : AbstractValidator<AddProductVariantCommand>
    {
        public AddProductVariantCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Ürün bilgisi boş olamaz.");

            RuleFor(x => x.Size)
                .NotEmpty().WithMessage("Beden bilgisi boş olamaz.")
                .MaximumLength(20).WithMessage("Beden en fazla 20 karakter olabilir.");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Renk bilgisi boş olamaz.")
                .MaximumLength(50).WithMessage("Renk en fazla 50 karakter olabilir.");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("SKU bilgisi boş olamaz.")
                .MaximumLength(50).WithMessage("SKU en fazla 50 karakter olabilir.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalıdır.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok adedi negatif olamaz.");
        }
    }
}
