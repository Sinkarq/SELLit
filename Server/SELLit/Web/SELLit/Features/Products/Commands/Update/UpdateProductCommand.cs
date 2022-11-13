using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;
using AutoMapper;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure.Extensions;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Features.Products.Commands.Update;

public sealed class UpdateProductCommand : IRequest<OneOf<UpdateProductCommandResponseModel, NotFound, Forbidden>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Title { get; set; } = "Unknown";

    public string Description { get; set; } = "Unknown";

    public string Location { get; set; } = "Unknown";

    public string PhoneNumber { get; set; } = "Unknown";

    public double Price { get; set; } 

    public DeliveryResponsibility DeliveryResponsibility { get; set; }
    
    public sealed class UpdateProductCommandHandler : 
        IRequestHandler<UpdateProductCommand, OneOf<UpdateProductCommandResponseModel, NotFound, Forbidden>>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly IMapper mapper;
        private readonly ICurrentUser currentUser;
        private readonly ILogger<UpdateProductCommandHandler> logger;

        public UpdateProductCommandHandler(
            IDeletableEntityRepository<Product> productRepository,
            IMapper mapper,
            ICurrentUser currentUser,
            ILogger<UpdateProductCommandHandler> logger)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.currentUser = currentUser;
            this.logger = logger;
        }

        public async ValueTask<OneOf<UpdateProductCommandResponseModel, NotFound, Forbidden>> Handle(
            UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await this.productRepository.Collection().FindAsync(request.Id);

            if (product is null)
            {
                return new NotFound();
            }

            if (product.UserId != this.currentUser.UserId)
            {
                return new Forbidden();
            }

            product.Title = request.Title;
            product.Description = request.Description;
            product.Location = request.Location;
            product.PhoneNumber = request.PhoneNumber;
            product.Price = request.Price;
            product.DeliveryResponsibility = request.DeliveryResponsibility;

            this.productRepository.Update(product);

            using (this.logger.EFQueryScope("Update Product"))
            {
                await this.productRepository.SaveChangesAsync(cancellationToken);
            }

            return this.mapper.Map<UpdateProductCommandResponseModel>(product);
        }
    }
}