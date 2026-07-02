using customer_service.Domain.Enums;
using FluentValidation;

namespace customer_service.Application.Features.Customer.Commands.AddCustomerAddress
{
    public class AddCustomerAddressCommandValidator : AbstractValidator<AddCustomerAddressCommand>
    {
        public AddCustomerAddressCommandValidator()
        {
            RuleFor(command => command.CustomerId)
                .NotEmpty().WithMessage("Müşteri id boş olamaz.");

            RuleFor(command => command.Type)
                .IsInEnum().WithMessage("Adres tipi geçersiz.");

            RuleFor(command => command.Title)
                .NotEmpty().WithMessage("Adres başlığı boş olamaz.")
                .MaximumLength(100).WithMessage("Adres başlığı en fazla 100 karakter olabilir.");

            RuleFor(command => command.RecipientFullName)
                .NotEmpty().WithMessage("Alıcı adı soyadı boş olamaz.")
                .MaximumLength(200).WithMessage("Alıcı adı soyadı en fazla 200 karakter olabilir.");

            RuleFor(command => command.PhoneNumber)
                .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
                .MaximumLength(30).WithMessage("Telefon numarası en fazla 30 karakter olabilir.");

            RuleFor(command => command.Country)
                .NotEmpty().WithMessage("Ülke boş olamaz.")
                .MaximumLength(100).WithMessage("Ülke en fazla 100 karakter olabilir.");

            RuleFor(command => command.City)
                .NotEmpty().WithMessage("Şehir boş olamaz.")
                .MaximumLength(100).WithMessage("Şehir en fazla 100 karakter olabilir.");

            RuleFor(command => command.District)
                .NotEmpty().WithMessage("İlçe boş olamaz.")
                .MaximumLength(100).WithMessage("İlçe en fazla 100 karakter olabilir.");

            RuleFor(command => command.AddressLine)
                .NotEmpty().WithMessage("Adres satırı boş olamaz.")
                .MaximumLength(500).WithMessage("Adres satırı en fazla 500 karakter olabilir.");

            RuleFor(command => command.PostalCode)
                .MaximumLength(20).WithMessage("Posta kodu en fazla 20 karakter olabilir.")
                .When(command => !string.IsNullOrWhiteSpace(command.PostalCode));

            RuleFor(command => command)
                .Must(command => !command.IsDefaultShipping || command.Type is AddressType.Shipping or AddressType.Both)
                .WithMessage("Varsayılan teslimat adresi için adres tipi Shipping veya Both olmalıdır.");

            RuleFor(command => command)
                .Must(command => !command.IsDefaultBilling || command.Type is AddressType.Billing or AddressType.Both)
                .WithMessage("Varsayılan fatura adresi için adres tipi Billing veya Both olmalıdır.");
        }
    }
}
