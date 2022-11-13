using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;
using Microsoft.EntityFrameworkCore;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure.Extensions;

namespace SELLit.Server.Features.Categories.Commands.Update;

public sealed class UpdateCategoryCommand : IRequest<OneOf<UpdateCategoryCommandResponseModel, NotFound, UniqueConstraintError>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public string Name { get; set; } = default!;
    
    public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand,OneOf<UpdateCategoryCommandResponseModel, NotFound, UniqueConstraintError>>
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly ILogger<UpdateCategoryCommandHandler> logger;

        public UpdateCategoryCommandHandler(
            IDeletableEntityRepository<Category> categoryRepository,
            ILogger<UpdateCategoryCommandHandler> logger)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
        }

        public async ValueTask<OneOf<UpdateCategoryCommandResponseModel, NotFound, UniqueConstraintError>> Handle(
            UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await this.categoryRepository
                    .AllAsNoTracking()
                    .TagWith("Check Name Availability - All Categories")
                    .AnyAsync(x => x.Name == request.Name, cancellationToken))
            {
                return new UniqueConstraintError("The Name provided is not available.");
            }

            var category = await this.categoryRepository
                .AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category is null)
            {
                return new NotFound();
            }

            category.Update(request.Name);

            this.categoryRepository.Update(category);

            using (this.logger.EFQueryScope("Update Category"))
            {
                await this.categoryRepository.SaveChangesAsync(cancellationToken);
            }

            return new UpdateCategoryCommandResponseModel()
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}