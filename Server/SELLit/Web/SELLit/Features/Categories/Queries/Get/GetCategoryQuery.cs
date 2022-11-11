using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SELLit.Data.Common.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace SELLit.Server.Features.Categories.Queries.Get;

public sealed class GetCategoryQuery : IRequest<GetCategoryQueryResponseModel>
{
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
                .TagWith("Get Category")
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