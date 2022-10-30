using System.Text.Json.Serialization;

namespace SELLit.Server.Features.Products.Queries.GetAll;

public sealed class GetAllProductsQueryResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Title { get; set; }
}