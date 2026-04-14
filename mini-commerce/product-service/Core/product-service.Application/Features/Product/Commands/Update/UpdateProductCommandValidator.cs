using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product_service.Application.Features.Product.Commands.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Ürün bilgisi boş olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .MaximumLength(150).WithMessage("Ürün adı en fazla 150 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Ürün açıklaması boş olamaz.")
                .MaximumLength(1000).WithMessage("Ürün açıklaması en fazla 1000 karakter olabilir.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marka bilgisi boş olamaz.")
                .MaximumLength(100).WithMessage("Marka en fazla 100 karakter olabilir.");

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori bilgisi boş olamaz.")
                .MaximumLength(100).WithMessage("Kategori en fazla 100 karakter olabilir.");
        }
    }
}
