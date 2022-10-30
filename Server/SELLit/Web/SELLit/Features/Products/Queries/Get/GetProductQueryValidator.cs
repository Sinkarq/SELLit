using FluentValidation;
using SELLit.Common;
using SELLit.Server.Features.Products.Queries.Get;

public sealed class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}