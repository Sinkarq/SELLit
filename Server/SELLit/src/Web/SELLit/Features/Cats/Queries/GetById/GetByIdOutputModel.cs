using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Cats.Queries.GetById;

public class GetByIdOutputModel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
}