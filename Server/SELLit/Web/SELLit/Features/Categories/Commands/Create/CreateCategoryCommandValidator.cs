using FluentValidation;
using SELLit.Common;

namespace SELLit.Server.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .Matches(@"^[A-Za-z\s,]*$")
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}