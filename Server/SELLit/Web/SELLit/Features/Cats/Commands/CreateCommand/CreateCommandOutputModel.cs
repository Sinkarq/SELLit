using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Cats.Commands.CreateCommand;

public class CreateCommandOutputModel : IMapFrom<Cat>
{
    public int Id { get; set; }
    public string Name { get; set; }
}