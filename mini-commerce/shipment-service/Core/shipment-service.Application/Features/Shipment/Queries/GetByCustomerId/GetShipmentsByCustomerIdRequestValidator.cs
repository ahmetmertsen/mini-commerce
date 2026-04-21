using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Queries.GetByCustomerId
{
    public class GetShipmentsByCustomerIdRequestValidator : AbstractValidator<GetShipmentsByCustomerIdRequest>
    {
        public GetShipmentsByCustomerIdRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Müşteri bilgisi boş olamaz.");
        }

    }
}
