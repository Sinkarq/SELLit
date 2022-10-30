using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure.Mapping.Interfaces;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Features.Products.Commands.Create;

public sealed class CreateProductCommand : IRequest<CreateProductCommandResponseModel>, IMapTo<Product>
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public string PhoneNumber { get; set; }

    public double Price { get; set; }

    public DeliveryResponsibility DeliveryResponsibility { get; set; }

    [JsonConverter(typeof(HashidsJsonConverter))]
    public int CategoryId { get; set; }

    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponseModel>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;

        public CreateProductCommandHandler(
            IDeletableEntityRepository<Product> productRepository, 
            IMapper mapper, 
            ICurrentUser currentUser)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.currentUser = currentUser;
        }

        public async Task<CreateProductCommandResponseModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = this.mapper.Map<Product>(request);
            product.UserId = this.currentUser.UserId;

            await this.productRepository.AddAsync(product, cancellationToken);
            await this.productRepository.SaveChangesAsync(cancellationToken);

            return new CreateProductCommandResponseModel
            {
                Id = product.Id
            };
        }
    }
}