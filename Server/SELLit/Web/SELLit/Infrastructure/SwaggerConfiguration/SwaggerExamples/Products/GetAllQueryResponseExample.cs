using SELLit.Server.Features.Categories.Queries.GetAll;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Products;

public class GetAllQueryResponseExample : IExamplesProvider<GetAllCategoriesQueryResponseModel>
{
    public GetAllCategoriesQueryResponseModel GetExamples() =>
        new()
        {
            Id = 69,
            Name = "Example name"
        };
}