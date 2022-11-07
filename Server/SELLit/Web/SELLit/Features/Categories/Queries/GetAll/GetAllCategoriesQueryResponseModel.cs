using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;

namespace SELLit.Server.Features.Categories.Queries.GetAll;

public sealed class GetAllCategoriesQueryResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public string Name { get; set; } = "Unknown";
}