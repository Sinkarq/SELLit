using FluentValidation;
using SELLit.Common;

namespace SELLit.Server.Features.Category.Commands.Delete;

public sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
        
        RuleFor(x => x.HardDelete)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}