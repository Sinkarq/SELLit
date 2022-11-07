using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;

namespace SELLit.Server.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommandResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
    
    public string Name { get; set; } = "Unknown";
}