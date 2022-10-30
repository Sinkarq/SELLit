using System.Text.Json.Serialization;

namespace SELLit.Server.Features.Categories.Commands.Create;

public sealed class CreateCategoryCommandResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
}