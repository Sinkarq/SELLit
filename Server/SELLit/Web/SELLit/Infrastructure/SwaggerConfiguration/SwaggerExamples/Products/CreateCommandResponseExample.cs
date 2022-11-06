using SELLit.Server.Features.Products.Commands.Create;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Products;

public class CreateCommandResponseExample : IExamplesProvider<CreateProductCommandResponseModel>
{
    public CreateProductCommandResponseModel GetExamples() =>
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