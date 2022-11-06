using SELLit.Server.Features.Categories.Queries.Get;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class GetQueryResponseExample : IExamplesProvider<GetCategoryQueryResponseModel>
{
    public GetCategoryQueryResponseModel GetExamples() =>
        new()
        {
            Id = 69,
            Name = "First name"
        };
}