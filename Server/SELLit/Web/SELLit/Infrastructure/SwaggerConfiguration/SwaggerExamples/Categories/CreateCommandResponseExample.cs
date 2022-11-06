using SELLit.Server.Features.Categories.Commands.Create;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class CreateCommandResponseExample : IExamplesProvider<CreateCategoryCommandResponseModel>
{
    public CreateCategoryCommandResponseModel GetExamples() =>
        new()
        {
            Id = 69,
            Name = "Example name"
        };
}