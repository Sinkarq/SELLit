using AspNetCore.Hashids.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Category.Queries.Get;

public sealed class GetCategoryQuery : IRequest<OneOf<GetCategoryQueryResponseModel, NotFound>>
{
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }

    public sealed class GetCategoryQueryValidator : IRequestHandler<GetCategoryQuery, OneOf<GetCategoryQueryResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Data.Models.Category> categoryRepository;

        public GetCategoryQueryValidator(
            IDeletableEntityRepository<Data.Models.Category> categoryRepository)
            => this.categoryRepository = categoryRepository;

        public async Task<OneOf<GetCategoryQueryResponseModel, NotFound>> Handle(GetCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var category = await this.categoryRepository
                .AllAsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new GetCategoryQueryResponseModel
                {
                    Name = x.Name
                }).FirstOrDefaultAsync(cancellationToken);

            if (category is null)
            {
                return new NotFound();
            }

            return category;
        }
    }
}