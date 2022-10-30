using MediatR;
using SELLit.Data.Common.Repositories;

namespace SELLit.Server.Features.Category.Commands.Create;

public sealed class CreateCategoryCommand : IRequest<CreateCategoryCommandResponseModel>
{
    public string Name { get; set; }

    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponseModel>
    {
        private readonly IDeletableEntityRepository<Data.Models.Category> categoryRepository;

        public CreateCategoryCommandHandler(IDeletableEntityRepository<Data.Models.Category> categoryRepository) 
            => this.categoryRepository = categoryRepository;

        public async Task<CreateCategoryCommandResponseModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Data.Models.Category(request.Name);

            await this.categoryRepository.AddAsync(category, cancellationToken);
            await this.categoryRepository.SaveChangesAsync(cancellationToken);

            return new CreateCategoryCommandResponseModel
            {
                Id = category.Id
            };
        }
    }
}