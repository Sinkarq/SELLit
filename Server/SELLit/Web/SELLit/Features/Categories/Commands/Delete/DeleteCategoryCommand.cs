using AspNetCore.Hashids.Mvc;
using Microsoft.AspNetCore.Mvc;

using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure.Extensions;

namespace SELLit.Server.Features.Categories.Commands.Delete;

public sealed class DeleteCategoryCommand : IRequest<OneOf<DeleteCategoryCommandResponseModel, NotFound>>
{
    [ModelBinder(typeof(HashidsModelBinder))]
    public int Id { get; set; }
    
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OneOf<DeleteCategoryCommandResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly ILogger<DeleteCategoryCommandHandler> logger;

        public DeleteCategoryCommandHandler(
            IDeletableEntityRepository<Category> categoryRepository,
            ILogger<DeleteCategoryCommandHandler> logger)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

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

            using (this.logger.EFQueryScope("Delete Category"))
            {
                await this.categoryRepository.SaveChangesAsync(cancellationToken);
            }

            return new DeleteCategoryCommandResponseModel();
        }
    }
}