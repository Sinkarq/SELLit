using AspNetCore.Hashids.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Categories.Commands.Delete;

public sealed class DeleteCategoryCommand : IRequest<OneOf<DeleteCategoryCommandResponseModel, NotFound>>
{
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }
    
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OneOf<DeleteCategoryCommandResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public DeleteCategoryCommandHandler(IDeletableEntityRepository<Category> categoryRepository) 
            => this.categoryRepository = categoryRepository;

        public async Task<OneOf<DeleteCategoryCommandResponseModel, NotFound>> Handle(
            DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            // TODO: Test query and delete entity with only ID
            var category = await this.categoryRepository
                .Collection().FindAsync(request.Id);

            if (category is null)
            {
                return new NotFound();
            }
            
            this.categoryRepository.Delete(category);

            await this.categoryRepository.SaveChangesAsync(cancellationToken);

            return new DeleteCategoryCommandResponseModel();
        }
    }
}