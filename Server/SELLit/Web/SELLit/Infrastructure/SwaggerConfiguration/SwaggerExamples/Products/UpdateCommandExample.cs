using SELLit.Server.Features.Products.Commands.Update;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Products;

public class UpdateCommandExample : IExamplesProvider<UpdateProductCommand>
{
    public UpdateProductCommand GetExamples() =>
        new()
        {
            Id = 69,
            Title = "Example title",
            Description = "Example description",
            Location = "Example location",
            PhoneNumber = "Example phone number",
            Price = 69.420,
            DeliveryResponsibility = DeliveryResponsibility.Buyer,
        };
}