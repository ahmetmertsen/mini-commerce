using FluentValidation;

namespace customer_service.Application.Features.Customer.Commands.CreateGuestCustomer
{
    public class CreateGuestCustomerCommandValidator : AbstractValidator<CreateGuestCustomerCommand>
    {
        public CreateGuestCustomerCommandValidator()
        {
            RuleFor(command => command.FullName)
                .NotEmpty().WithMessage("Ad soyad bos olamaz.")
                .MaximumLength(200).WithMessage("Ad soyad en fazla 200 karakter olabilir.");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email bos olamaz.")
                .EmailAddress().WithMessage("Email formati gecersiz.")
                .MaximumLength(200).WithMessage("Email en fazla 200 karakter olabilir.");

            RuleFor(command => command.PhoneNumber)
                .MaximumLength(30).WithMessage("Telefon numarasi en fazla 30 karakter olabilir.")
                .When(command => !string.IsNullOrWhiteSpace(command.PhoneNumber));
        }
    }
}
