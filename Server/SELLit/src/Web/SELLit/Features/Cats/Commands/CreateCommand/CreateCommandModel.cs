using AutoMapper;
using MediatR;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure.Extensions;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Cats.Commands.CreateCommand;

public class CreateCommandModel : 
    IRequest<CreateCommandOutputModel>, 
    IMapTo<Cat>
{
    public string Name { get; set; }
    
    public class CreateCommandModelHandler : IRequestHandler<CreateCommandModel, CreateCommandOutputModel>
    {
        private readonly IDeletableEntityRepository<Cat> catRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CreateCommandModelHandler(IMapper mapper, IDeletableEntityRepository<Cat> catRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.catRepository = catRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateCommandOutputModel> Handle(CreateCommandModel request, CancellationToken cancellationToken)
        {
            var cat = this.mapper.Map<Cat>(request);
            var userId = this.httpContextAccessor.HttpContext?.User.GetId();
            cat.UserId = userId!;

            await this.catRepository.AddAsync(cat, cancellationToken);

            await this.catRepository.SaveChangesAsync(cancellationToken);

            var outputModel = this.mapper.Map<CreateCommandOutputModel>(cat);

            return outputModel;
        }
    }
}