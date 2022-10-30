using FluentValidation;

namespace SELLit.Server.Features.Identity.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private const string ValidationMessage = "Please fill in {PropertyName}.";

    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessage)
            .EmailAddress().WithMessage("Please fill in valid {PropertyName}.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessage)
            .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters long.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessage);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessage);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationMessage);

    }
}