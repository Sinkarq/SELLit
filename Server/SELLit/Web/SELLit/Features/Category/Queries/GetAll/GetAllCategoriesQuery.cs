using MediatR;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Category.Queries.GetAll;

public sealed class GetAllCategoriesQuery : IRequest<IEnumerable<GetAllCategoriesQueryResponseModel>>
{
    public sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<GetAllCategoriesQueryResponseModel>>
    {
        private readonly IDeletableEntityRepository<Data.Models.Category> categoryRepository;

        public GetAllCategoriesQueryHandler(IDeletableEntityRepository<Data.Models.Category> categoryRepository) => this.categoryRepository = categoryRepository;

        public async Task<IEnumerable<GetAllCategoriesQueryResponseModel>> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
            => this.categoryRepository
                .AllAsNoTracking()
                .Select(x => new GetAllCategoriesQueryResponseModel
            {
                Id = x.Id,
                Name = x.Name
            });
    }
}