using AspNetCore.Hashids.Mvc;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Products.Queries.Get;

public sealed class GetProductQuery : IRequest<GetProductQueryResponseModel>
{
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }

    public sealed class
        GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductQueryResponseModel>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IMapper mapper;

        public GetProductQueryHandler(IDeletableEntityRepository<Product> productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<GetProductQueryResponseModel> Handle(GetProductQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await this.productRepository
                .AllAsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new GetProductQueryResponseModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Location = x.Location,
                    PhoneNumber = x.PhoneNumber,
                    Price = x.Price,
                    DeliveryResponsibility = x.DeliveryResponsibility,
                    CategoryName = x.Category.Name
                }).FirstOrDefaultAsync(cancellationToken);

            return entity;
        }
    }
}