using FluentValidation;
using static SELLit.Common.ValidationConstants;

namespace SELLit.Server.Features.Identity.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommandRequestModel>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessage)
            .NotNull().WithMessage(ValidationMessage);

        RuleFor(x => x.Password)
            .NotNull().WithMessage(ValidationMessage)
            .NotEmpty().WithMessage(ValidationMessage);
    }
}