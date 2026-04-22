using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Features.Notification.Queries.GetByCustomerId
{
    public class GetNotificationsByCustomerIdRequestValidator : AbstractValidator<GetNotificationsByCustomerIdRequest>
    {
        public GetNotificationsByCustomerIdRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Müşteri bilgisi boş olamaz.");
        }
    }
}
