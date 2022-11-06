using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommand : IRequest<OneOf<CreateCategoryCommandResponseModel, UniqueConstraintError>>
{
    public string Name { get; set; }

    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand,
        OneOf<CreateCategoryCommandResponseModel, UniqueConstraintError>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CreateCategoryCommandHandler(IDeletableEntityRepository<Category> categoryRepository)
            => this.categoryRepository = categoryRepository;

        public async Task<OneOf<CreateCategoryCommandResponseModel, UniqueConstraintError>> Handle(
            CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await this.categoryRepository.AllAsNoTrackingWithDeleted()
                    .AnyAsync(x => x.Name == request.Name, cancellationToken))
            {
                return new UniqueConstraintError("The Name provided is not available.");
            }
            
            var category = new Category(request.Name);
            await this.categoryRepository.AddAsync(category, cancellationToken);
            await this.categoryRepository.SaveChangesAsync(cancellationToken);


            return new CreateCategoryCommandResponseModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}