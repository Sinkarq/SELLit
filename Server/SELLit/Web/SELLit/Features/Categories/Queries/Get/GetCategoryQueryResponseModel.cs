using System.Text.Json.Serialization;

namespace SELLit.Server.Features.Categories.Queries.Get;

public sealed class GetCategoryQueryResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Name { get; set; }
}