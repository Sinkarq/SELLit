using SELLit.Server.Features.Categories.Commands.Update;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class UpdateCommandResponseExample : IExamplesProvider<UpdateCategoryCommandResponseModel>
{
    public UpdateCategoryCommandResponseModel GetExamples() =>
        new()
        {
            Id = 1,
            Name = "Updated name example"
        };
}