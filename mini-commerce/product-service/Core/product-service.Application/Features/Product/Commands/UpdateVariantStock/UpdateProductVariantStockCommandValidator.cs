using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.UpdateVariantStock
{
    public class UpdateProductVariantStockCommandValidator : AbstractValidator<UpdateProductVariantStockCommand>
    {
        public UpdateProductVariantStockCommandValidator()
        {
            RuleFor(x => x.VariantId)
                .NotEmpty().WithMessage("Varyant bilgisi boş olamaz.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok adedi negatif olamaz.");
        }
    }
}
