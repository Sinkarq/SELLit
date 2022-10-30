using System.Text.Json.Serialization;

namespace SELLit.Server.Features.Products.Commands.Create;

public sealed class CreateProductCommandResponseModel
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }
}