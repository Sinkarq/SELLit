using SELLit.Server.Features.Categories.Commands.Create;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class CreateCommandExample : IExamplesProvider<CreateCategoryCommand>
{
    public CreateCategoryCommand GetExamples() =>
        new()
        {
            Name = "Example name"
        };
}