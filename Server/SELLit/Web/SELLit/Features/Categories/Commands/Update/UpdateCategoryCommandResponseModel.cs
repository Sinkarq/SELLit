using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;

namespace SELLit.Server.Features.Categories.Commands.Update;

public sealed class UpdateCategoryCommandResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public string Name { get; set; } = "Unknown";
}