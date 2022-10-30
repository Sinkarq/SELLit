using FluentValidation;
using SELLit.Common;

namespace SELLit.Server.Features.Products.Commands.Delete;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}