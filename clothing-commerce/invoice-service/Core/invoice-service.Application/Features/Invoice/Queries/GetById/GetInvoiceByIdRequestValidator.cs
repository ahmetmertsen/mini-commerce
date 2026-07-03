using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Queries.GetById
{
    public class GetInvoiceByIdRequestValidator : AbstractValidator<GetInvoiceByIdRequest>
    {
        public GetInvoiceByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Fatura bilgisi boş olamaz.");
        }
    }
}
