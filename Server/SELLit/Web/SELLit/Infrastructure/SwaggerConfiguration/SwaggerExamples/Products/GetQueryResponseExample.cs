using SELLit.Server.Features.Products.Queries.Get;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Products;

public class GetQueryResponseExample : IExamplesProvider<GetProductQueryResponseModel>
{
    public GetProductQueryResponseModel GetExamples() =>
        new()
        {
            Id = 69,
            Title = "Example title",
            Description = "Example description",
            Location = "Example location",
            PhoneNumber = "Example phone number",
            Price = 69.420,
            DeliveryResponsibility = DeliveryResponsibility.Buyer,
            CategoryName = "Example category name"
        };
}