using FluentValidation;
using SELLit.Common;

namespace SELLit.Server.Features.Category.Commands.Rename;

public sealed class RenameCategoryCommandValidator : AbstractValidator<RenameCategoryCommand>
{
    public RenameCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);

        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}