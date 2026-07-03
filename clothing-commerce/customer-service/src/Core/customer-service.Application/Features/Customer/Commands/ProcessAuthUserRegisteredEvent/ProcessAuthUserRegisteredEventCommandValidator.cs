using FluentValidation;

namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserRegisteredEvent
{
    public class ProcessAuthUserRegisteredEventCommandValidator : AbstractValidator<ProcessAuthUserRegisteredEventCommand>
    {
        public ProcessAuthUserRegisteredEventCommandValidator()
        {
            RuleFor(command => command.MessageId)
                .NotEmpty().WithMessage("Message id bos olamaz.");

            RuleFor(command => command.AuthUserId)
                .NotEmpty().WithMessage("Auth user id bos olamaz.");

            RuleFor(command => command.CorrelationId)
                .NotEmpty().WithMessage("Correlation id bos olamaz.");

            RuleFor(command => command.FullName)
                .NotEmpty().WithMessage("Ad soyad bos olamaz.")
                .MaximumLength(200).WithMessage("Ad soyad en fazla 200 karakter olabilir.");

            RuleFor(command => command.Email)
                .NotEmpty().WithMessage("Email bos olamaz.")
                .EmailAddress().WithMessage("Email formati gecersiz.")
                .MaximumLength(200).WithMessage("Email en fazla 200 karakter olabilir.");
        }
    }
}
