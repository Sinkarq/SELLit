using AspNetCore.Hashids.Mvc;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Products.Queries.Get;

public sealed class GetProductQuery : IRequest<OneOf<GetProductQueryResponseModel, NotFound>>
{
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }

    public sealed class GetProductQueryHandler : IRequestHandler<GetProductQuery, OneOf<GetProductQueryResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IMapper mapper;

        public GetProductQueryHandler(IDeletableEntityRepository<Product> productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<OneOf<GetProductQueryResponseModel, NotFound>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var entity = await this.productRepository.Collection().FindAsync(request.Id);

            if (entity is null)
            {
                return new NotFound();
            }

            return this.mapper.Map<GetProductQueryResponseModel>(entity);
        }
    }
}