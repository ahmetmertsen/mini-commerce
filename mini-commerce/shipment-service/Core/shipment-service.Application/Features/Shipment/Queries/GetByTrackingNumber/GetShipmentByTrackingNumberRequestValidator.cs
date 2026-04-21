using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Queries.GetByTrackingNumber
{
    public class GetShipmentByTrackingNumberRequestValidator : AbstractValidator<GetShipmentByTrackingNumberRequest>
    {
        public GetShipmentByTrackingNumberRequestValidator() 
        {
            RuleFor(x => x.TrackingNumber)
                .NotEmpty().WithMessage("Takip numarası boş olamaz.")
                .Matches(@"^\d+$").WithMessage("Takip numarası sadece rakamlardan oluşmalıdır.");
        }
    }
}
