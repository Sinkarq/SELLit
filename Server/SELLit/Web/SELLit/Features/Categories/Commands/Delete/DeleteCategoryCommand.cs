using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;

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

        public async ValueTask<OneOf<DeleteCategoryCommandResponseModel, NotFound>> Handle(
            DeleteCategoryCommand request, CancellationToken cancellationToken)
        { 
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