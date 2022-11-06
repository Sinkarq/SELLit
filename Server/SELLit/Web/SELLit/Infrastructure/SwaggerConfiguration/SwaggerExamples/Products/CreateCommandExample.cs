using SELLit.Server.Features.Products.Commands.Create;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Products;

public class CreateCommandExample : IExamplesProvider<CreateProductCommand>
{
    public CreateProductCommand GetExamples() =>
        new()
        {
            Title = "Example title",
            Description = "Example description",
            Location = "Example location",
            PhoneNumber = "Example phone number",
            Price = 69.420,
            DeliveryResponsibility = DeliveryResponsibility.Buyer,
        };
}