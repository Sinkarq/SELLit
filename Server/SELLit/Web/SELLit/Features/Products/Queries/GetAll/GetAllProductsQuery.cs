using MediatR;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Products.Queries.GetAll;

public sealed class GetAllProductsQuery : IRequest<IEnumerable<GetAllProductsQueryResponseModel>>
{
    public sealed class
        GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<GetAllProductsQueryResponseModel>>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;

        public GetAllProductsQueryHandler(IDeletableEntityRepository<Product> productRepository)
            => this.productRepository = productRepository;

        public async Task<IEnumerable<GetAllProductsQueryResponseModel>> Handle(
            GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var entities = this.productRepository
                .AllAsNoTracking()
                .Select(x => new GetAllProductsQueryResponseModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                });

            return entities;
        }
    }
}