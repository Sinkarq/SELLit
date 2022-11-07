using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Categories.Queries.Get;

public sealed class GetCategoryQuery : IRequest<GetCategoryQueryResponseModel>
{
    [FromRoute]
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }

    public sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, GetCategoryQueryResponseModel>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public GetCategoryQueryHandler(
            IDeletableEntityRepository<Category> categoryRepository)
            => this.categoryRepository = categoryRepository;

        public async ValueTask<GetCategoryQueryResponseModel> Handle(GetCategoryQuery request,
            CancellationToken cancellationToken)
        {
            var category = await this.categoryRepository
                .AllAsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new GetCategoryQueryResponseModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).FirstOrDefaultAsync(cancellationToken);

            return category!;
        }
    }
}