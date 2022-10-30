using System.Text.Json.Serialization;
using MediatR;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Categories.Commands.Update;

public sealed class UpdateCategoryCommand : IRequest<OneOf<UpdateCategoryCommandResponseModel, NotFound>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public sealed class RenameCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand,OneOf<UpdateCategoryCommandResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public RenameCategoryCommandHandler(IDeletableEntityRepository<Category> categoryRepository) => this.categoryRepository = categoryRepository;

        public async Task<OneOf<UpdateCategoryCommandResponseModel, NotFound>> Handle(
            UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await this.categoryRepository
                .Collection().FindAsync(request.Id, cancellationToken);

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