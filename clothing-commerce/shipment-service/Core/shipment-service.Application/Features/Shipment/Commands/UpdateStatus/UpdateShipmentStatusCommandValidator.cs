using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Features.Shipment.Commands.UpdateStatus
{
    public class UpdateShipmentStatusCommandValidator : AbstractValidator<UpdateShipmentStatusCommand>
    {
        public UpdateShipmentStatusCommandValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Kargo bilgisi boş olamaz.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçersiz kargo durumu.");
        }
    }
}
