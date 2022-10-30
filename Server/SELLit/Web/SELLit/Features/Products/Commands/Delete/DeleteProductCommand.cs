using System.Text.Json.Serialization;
using MediatR;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Features.Products.Commands.Delete;

public sealed class DeleteProductCommand : IRequest<OneOf<DeleteProductCommandResponseModel, NotFound, Unauthorized>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public bool HardDelete { get; set; }

    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand,
        OneOf<DeleteProductCommandResponseModel, NotFound, Unauthorized>>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly ICurrentUser currentUser;

        public DeleteProductCommandHandler(IDeletableEntityRepository<Product> productRepository, ICurrentUser currentUser)
        {
            this.productRepository = productRepository;
            this.currentUser = currentUser;
        }

        public async Task<OneOf<DeleteProductCommandResponseModel, NotFound, Unauthorized>> Handle(
            DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.productRepository
                .Collection()
                .FindAsync(request.Id);

            if (entity is null)
            {
                return new NotFound();
            }

            if (entity.UserId != currentUser.UserId)
            {
                return new Unauthorized();
            }

            if (request.HardDelete)
            { 
                this.productRepository.HardDelete(entity);
            }
            else
            {
                this.productRepository.Delete(entity);
            }

            await this.productRepository.SaveChangesAsync(cancellationToken);

            return new DeleteProductCommandResponseModel();
        }
    }
}