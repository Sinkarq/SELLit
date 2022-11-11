using Microsoft.EntityFrameworkCore;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure.Extensions;

namespace SELLit.Server.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommand : IRequest<OneOf<CreateCategoryCommandResponseModel, UniqueConstraintError>>
{
    public string Name { get; set; } = default!;

    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand,
        OneOf<CreateCategoryCommandResponseModel, UniqueConstraintError>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly ILogger<CreateCategoryCommandHandler> logger;

        public CreateCategoryCommandHandler(
            IDeletableEntityRepository<Category> categoryRepository,
            ILogger<CreateCategoryCommandHandler> logger)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

        public async ValueTask<OneOf<CreateCategoryCommandResponseModel, UniqueConstraintError>> Handle(
            CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await this.categoryRepository
                    .AllAsNoTrackingWithDeleted()
                    .TagWith("Check Name Availability - All Categories")
                    .AnyAsync(x => x.Name == request.Name, cancellationToken))
            {
                return new UniqueConstraintError("The Name provided is not available.");
            }

            var category = new Category(request.Name);
            await this.categoryRepository.AddAsync(category, cancellationToken);

            using (this.logger.EFQueryScope("Create Category"))
            {
                await this.categoryRepository.SaveChangesAsync(cancellationToken);
            }

            return new CreateCategoryCommandResponseModel
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}