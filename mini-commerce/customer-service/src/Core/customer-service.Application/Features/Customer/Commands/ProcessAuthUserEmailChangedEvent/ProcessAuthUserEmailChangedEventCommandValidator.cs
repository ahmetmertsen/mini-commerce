using FluentValidation;

namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserEmailChangedEvent
{
    public class ProcessAuthUserEmailChangedEventCommandValidator : AbstractValidator<ProcessAuthUserEmailChangedEventCommand>
    {
        public ProcessAuthUserEmailChangedEventCommandValidator()
        {
            RuleFor(command => command.MessageId)
                .NotEmpty().WithMessage("Message id bos olamaz.");

            RuleFor(command => command.AuthUserId)
                .NotEmpty().WithMessage("Auth user id bos olamaz.");

            RuleFor(command => command.CorrelationId)
                .NotEmpty().WithMessage("Correlation id bos olamaz.");

            RuleFor(command => command.OldEmail)
                .NotEmpty().WithMessage("Eski email bos olamaz.")
                .EmailAddress().WithMessage("Eski email formati gecersiz.")
                .MaximumLength(200).WithMessage("Eski email en fazla 200 karakter olabilir.");

            RuleFor(command => command.NewEmail)
                .NotEmpty().WithMessage("Yeni email bos olamaz.")
                .EmailAddress().WithMessage("Yeni email formati gecersiz.")
                .MaximumLength(200).WithMessage("Yeni email en fazla 200 karakter olabilir.")
                .Must((command, newEmail) => !string.Equals(command.OldEmail, newEmail, StringComparison.OrdinalIgnoreCase))
                .WithMessage("Yeni email eski email ile ayni olamaz.");
        }
    }
}
