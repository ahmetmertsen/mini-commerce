using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Features.Notification.Queries.GetById
{
    public class GetNotificationByIdRequestValidator : AbstractValidator<GetNotificationByIdRequest>
    {
        public GetNotificationByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Bildirim bilgisi boş olamaz.");
        }
    }
}
