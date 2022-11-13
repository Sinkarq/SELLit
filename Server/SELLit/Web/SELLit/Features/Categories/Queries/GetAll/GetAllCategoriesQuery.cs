using Microsoft.EntityFrameworkCore;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Categories.Queries.GetAll;

public sealed class GetAllCategoriesQuery : IRequest<IEnumerable<GetAllCategoriesQueryResponseModel>>
{
    public static readonly GetAllCategoriesQuery Instance = new();

    public sealed class
        GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery,
            IEnumerable<GetAllCategoriesQueryResponseModel>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public GetAllCategoriesQueryHandler(IDeletableEntityRepository<Category> categoryRepository)
            => this.categoryRepository = categoryRepository;

        public async ValueTask<IEnumerable<GetAllCategoriesQueryResponseModel>> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
            => await this.categoryRepository
                .AllAsNoTracking()
                .TagWith("Get All Categories")
                .Select(x => new GetAllCategoriesQueryResponseModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync(cancellationToken);
    }
}