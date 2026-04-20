using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Commands.Create
{
    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator() 
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Sipariş bilgisi boş olamaz.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Müşteri bilgisi boş olamaz.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Fatura tutarı sıfırdan büyük olmalıdır.");
        }
    }
}
