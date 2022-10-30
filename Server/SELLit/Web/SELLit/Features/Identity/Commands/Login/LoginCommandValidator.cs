using FluentValidation;
using static SELLit.Common.ValidationConstants;

namespace SELLit.Server.Features.Identity.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessage);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessage);
    }
}