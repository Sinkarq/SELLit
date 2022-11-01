namespace SELLit.Server.Features.Cats.Queries.GetById;

using FluentValidation;

public class GetByIdQueryModelValidator : AbstractValidator<GetByIdQueryModel>
{
    public GetByIdQueryModelValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();
    }
}