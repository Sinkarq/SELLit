using SELLit.Server.Features.Categories.Commands.Update;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class UpdateCommandExample : IExamplesProvider<UpdateCategoryCommand>
{
    public UpdateCategoryCommand GetExamples() =>
        new()
        {
            Id = 1,
            Name = "420"
        };
}