using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Queries.GetByOrderId
{
    public class GetInvoicesByOrderIdRequestValidator : AbstractValidator<GetInvoicesByOrderIdRequest>
    {
        public GetInvoicesByOrderIdRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Sipariş bilgisi boş olamaz.");
        }
    }
}
