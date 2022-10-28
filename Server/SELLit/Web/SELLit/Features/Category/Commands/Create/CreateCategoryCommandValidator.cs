using FluentValidation;
using SELLit.Common;
using SELLit.Server.Features.Category.Commands.Create;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}