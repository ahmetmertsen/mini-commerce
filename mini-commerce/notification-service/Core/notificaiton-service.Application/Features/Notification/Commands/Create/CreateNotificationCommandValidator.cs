using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Application.Features.Notification.Commands.Create
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Müşteri bilgisi boş olamaz.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Geçersiz bildirim tipi.");

            RuleFor(x => x.Channel)
                .IsInEnum().WithMessage("Geçersiz bildirim kanalı.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Bildirim başlığı boş olamaz.")
                .MaximumLength(200).WithMessage("Bildirim başlığı en fazla 200 karakter olabilir.");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Bildirim mesajı boş olamaz.")
                .MaximumLength(2000).WithMessage("Bildirim mesajı en fazla 2000 karakter olabilir.");
        }
    }
}
