using System.Text.Json.Serialization;
using AspNetCore.Hashids.Mvc;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Category.Commands.Delete;

public sealed class DeleteCategoryCommand : IRequest<OneOf<DeleteCategoryCommandResponseModel, NotFound>>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public bool HardDelete { get; set; }
    
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, OneOf<DeleteCategoryCommandResponseModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Data.Models.Category> categoryRepository;

        public DeleteCategoryCommandHandler(IDeletableEntityRepository<Data.Models.Category> categoryRepository) 
            => this.categoryRepository = categoryRepository;

        public async Task<OneOf<DeleteCategoryCommandResponseModel, NotFound>> Handle(
            DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await this.categoryRepository
                .AllAsNoTracking()
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (category is null)
            {
                return new NotFound();
            }
            
            if (request.HardDelete)
            {
                this.categoryRepository.HardDelete(category);
            }
            else
            {
                this.categoryRepository.Delete(category);
            }

            await this.categoryRepository.SaveChangesAsync(cancellationToken);

            return new DeleteCategoryCommandResponseModel();
        }
    }
}