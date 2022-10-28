using FluentValidation;
using static SELLit.Common.ValidationConstants;

namespace SELLit.Server.Features.Identity.Commands.Login;

public class LoginCommandModelValidator : AbstractValidator<LoginCommandModel>
{
    public LoginCommandModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessage)
            .NotNull().WithMessage(ValidationMessage);

        RuleFor(x => x.Password)
            .NotNull().WithMessage(ValidationMessage)
            .NotEmpty().WithMessage(ValidationMessage);
    }
}