using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Category.Commands.Rename;

public sealed class RenameCategoryCommand : IRequest<OneOf<RenameCategoryCommandResponseModel, NotFound>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public sealed class RenameCategoryCommandHandler : IRequestHandler<RenameCategoryCommand,OneOf<RenameCategoryCommandResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Data.Models.Category> categoryRepository;

        public RenameCategoryCommandHandler(IDeletableEntityRepository<Data.Models.Category> categoryRepository) => this.categoryRepository = categoryRepository;

        public async Task<OneOf<RenameCategoryCommandResponseModel, NotFound>> Handle(RenameCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await this.categoryRepository
                .AllAsNoTracking()
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
            {
                return new NotFound();
            }

            category.Rename(request.Name);

            this.categoryRepository.Update(category);
            await this.categoryRepository.SaveChangesAsync(cancellationToken);

            return new RenameCategoryCommandResponseModel();
        }
    }
}