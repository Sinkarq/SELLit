using FluentValidation;
using SELLit.Common;

namespace SELLit.Server.Features.Categories.Queries.Get;

public sealed class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage(ValidationConstants.ValidationMessage);
    }
}