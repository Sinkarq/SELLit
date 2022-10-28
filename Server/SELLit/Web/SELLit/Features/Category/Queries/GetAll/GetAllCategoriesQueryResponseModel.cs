using System.Text.Json.Serialization;

namespace SELLit.Server.Features.Category.Queries.GetAll;

public sealed class GetAllCategoriesQueryResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Name { get; set; }
}