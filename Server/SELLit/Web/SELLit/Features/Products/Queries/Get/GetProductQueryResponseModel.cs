using System.Text.Json.Serialization;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Products.Queries.Get;

public sealed class GetProductQueryResponseModel : IMapFrom<Product>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Title { get; set; }
}