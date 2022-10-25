using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using SELLit.Data.Common.Repositories;
using SELLit.Server.Infrastructure;

namespace SELLit.Server.Features.Cats.Queries.GetById;

public class GetByIdQueryModel :
    IRequest<OneOf<GetByIdOutputModel, NotFound>>
{
    public int Id { get; set; }

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQueryModel, OneOf<GetByIdOutputModel, NotFound>>
    {
        private readonly IDeletableEntityRepository<Cat> catRepository;

        public GetByIdQueryHandler(IDeletableEntityRepository<Cat> catRepository, IMapper mapper) => this.catRepository = catRepository;

        public async Task<OneOf<GetByIdOutputModel, NotFound>> Handle(GetByIdQueryModel request,
            CancellationToken cancellationToken)
        {
            var id = request.Id;

            var entity = await this.catRepository
                .All()
                .Where(x => x.Id == request.Id)
                .Select(x => new GetByIdOutputModel
                {
                    Name = x.Name,
                    Id = x.Id
                }).FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
            {
                return new NotFound();
            }

            return entity;
        }
    }
}