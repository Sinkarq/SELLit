using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Features.Products.Commands.Delete;

public sealed class DeleteProductCommand : IRequest<OneOf<DeleteProductCommandResponseModel, NotFound, Forbidden>>
{
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }

    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand,
        OneOf<DeleteProductCommandResponseModel, NotFound, Forbidden>>
    {
        private readonly IDeletableEntityRepository<Product> productRepository;
        private readonly ICurrentUser currentUser;

        public DeleteProductCommandHandler(IDeletableEntityRepository<Product> productRepository, ICurrentUser currentUser)
        {
            this.productRepository = productRepository;
            this.currentUser = currentUser;
        }

        public async ValueTask<OneOf<DeleteProductCommandResponseModel, NotFound, Forbidden>> Handle(
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
                return new Forbidden();
            }

            this.productRepository.Delete(entity);

            await this.productRepository.SaveChangesAsync(cancellationToken);

            return new DeleteProductCommandResponseModel();
        }
    }
}