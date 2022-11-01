using FluentValidation;

namespace SELLit.Server.Features.Identity.Commands.Login;

public class LoginCommandRequestModelValidator : AbstractValidator<LoginCommandRequestModel>
{
    private const string ValidationMessage = "Please fill in {PropertyName}.";

    public LoginCommandRequestModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessage)
            .NotNull().WithMessage(ValidationMessage);

        RuleFor(x => x.Password)
            .NotNull().WithMessage(ValidationMessage)
            .NotEmpty().WithMessage(ValidationMessage);
    }
}