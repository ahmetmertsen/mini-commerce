using FluentValidation;

namespace customer_service.Application.Features.Customer.Commands.CreateRegisteredCustomer
{
    public class CreateRegisteredCustomerCommandValidator : AbstractValidator<CreateRegisteredCustomerCommand>
    {
        public CreateRegisteredCustomerCommandValidator()
        {
            RuleFor(command => command.AuthUserId)
                .NotEmpty().WithMessage("Auth user id boş olamaz.");

            RuleFor(command => command.FirstName)
                .NotEmpty().WithMessage("Ad boş olamaz.")
                .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir.");

            RuleFor(command => command.LastName)
                .NotEmpty().WithMessage("Soyad boş olamaz.")
                .MaximumLength(100).WithMessage("Soyad en fazla 100 karakter olabilir.");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Email formatı geçersiz.")
                .MaximumLength(200).WithMessage("Email en fazla 200 karakter olabilir.");

            RuleFor(command => command.PhoneNumber)
                .MaximumLength(30).WithMessage("Telefon numarası en fazla 30 karakter olabilir.")
                .When(command => !string.IsNullOrWhiteSpace(command.PhoneNumber));
        }
    }
}
