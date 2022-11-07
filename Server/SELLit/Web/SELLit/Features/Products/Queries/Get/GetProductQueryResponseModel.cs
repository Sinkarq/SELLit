using System.Text.Json.Serialization;
using AspNetCore.Hashids.Json;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Products.Queries.Get;

public sealed class GetProductQueryResponseModel : IMapFrom<Product>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public string Title { get; set; } = "Unknown";

    public string Description { get; set; } = "Unknown";

    public string Location { get; set; } = "Unknown";

    public string PhoneNumber { get; set; } = "Unknown";

    public double Price { get; set; } 

    public DeliveryResponsibility DeliveryResponsibility { get; set; }
    
    public string CategoryName { get; set; } = "Unknown";
}