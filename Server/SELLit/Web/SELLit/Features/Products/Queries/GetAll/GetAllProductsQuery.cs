using Microsoft.EntityFrameworkCore;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Products.Queries.GetAll;

public sealed class GetAllProductsQuery : IRequest<IEnumerable<GetAllProductsQueryResponseModel>>
{
    public static readonly GetAllProductsQuery Instance = new();

    public sealed class
        GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<GetAllProductsQueryResponseModel>>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;

        public GetAllProductsQueryHandler(IDeletableEntityRepository<Product> productRepository)
            => this.productRepository = productRepository;

        public async ValueTask<IEnumerable<GetAllProductsQueryResponseModel>> Handle(
            GetAllProductsQuery request, CancellationToken cancellationToken) =>
            await this.productRepository
                .AllAsNoTracking()
                .Select(x => new GetAllProductsQueryResponseModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).ToListAsync(cancellationToken);
    }
}