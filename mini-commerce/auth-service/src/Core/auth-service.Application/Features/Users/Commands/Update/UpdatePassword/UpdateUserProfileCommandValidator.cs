using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auth_service.Application.Features.Users.Commands.Update.UpdatePassword
{
    public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        public UpdateUserPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Email formatı geçersiz.");
                
            RuleFor(x => x.VerificationCode)
                .NotEmpty().WithMessage("Şifre sıfırlama kodu boş olamaz.")
                .Length(6).WithMessage("Şifre sıfırlama kodu 6 haneli olmalıdır.")
                .Matches("^[0-9]{6}$").WithMessage("Şifre sıfırlama kodu sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.newPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.");

            RuleFor(x => x.newPasswordConfirmed)
                .NotEmpty().WithMessage("Yeni şifre tekrarı boş olamaz.")
                .MinimumLength(6).WithMessage("Yeni şifre tekrarı en az 6 karakter olmalıdır.")
                .Equal(x => x.newPassword).WithMessage("Şifreler uyuşmuyor.");
        }
    }
}
