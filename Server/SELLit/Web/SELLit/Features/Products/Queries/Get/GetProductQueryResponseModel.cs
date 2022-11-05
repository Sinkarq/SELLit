using System.Text.Json.Serialization;
using SELLit.Server.Infrastructure.Mapping.Interfaces;

namespace SELLit.Server.Features.Products.Queries.Get;

public sealed class GetProductQueryResponseModel : IMapFrom<Product>
{
    [JsonConverter(typeof(HashidsJsonConverter))]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public string PhoneNumber { get; set; }

    public double Price { get; set; }

    public DeliveryResponsibility DeliveryResponsibility { get; set; }
    
    public string CategoryName { get; set; }
}