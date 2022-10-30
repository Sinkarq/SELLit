using FluentValidation;
using SELLit.Common;
using SELLit.Server.Features.Products.Commands.Delete;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage(ValidationConstants.ValidationMessage);

        RuleFor(x => x.HardDelete)
            .NotNull()
            .NotEmpty()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}