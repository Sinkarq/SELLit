using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Categories.Commands.Update;

public sealed class UpdateCategoryCommand : IRequest<OneOf<UpdateCategoryCommandResponseModel, NotFound, UniqueConstraintError>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public sealed class RenameCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand,OneOf<UpdateCategoryCommandResponseModel, NotFound, UniqueConstraintError>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public RenameCategoryCommandHandler(IDeletableEntityRepository<Category> categoryRepository) => this.categoryRepository = categoryRepository;

        public async Task<OneOf<UpdateCategoryCommandResponseModel, NotFound, UniqueConstraintError>> Handle(
            UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await this.categoryRepository.AllAsNoTrackingWithDeleted()
                    .AnyAsync(x => x.Name == request.Name, cancellationToken))
            {
                return new UniqueConstraintError("The Name provided is not available.");
            }
            
            var category = await this.categoryRepository
                .Collection().FindAsync(request.Id);

            if (category is null)
            {
                return new NotFound();
            }

            category.Update(request.Name);

            this.categoryRepository.Update(category);
            await this.categoryRepository.SaveChangesAsync(cancellationToken);

            return new UpdateCategoryCommandResponseModel();
        }
    }
}