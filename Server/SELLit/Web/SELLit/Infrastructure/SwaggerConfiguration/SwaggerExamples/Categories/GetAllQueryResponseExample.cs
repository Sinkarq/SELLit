using SELLit.Server.Features.Categories.Queries.GetAll;
using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class GetAllQueryResponseExample : IExamplesProvider<List<GetAllCategoriesQueryResponseModel>>
{
    public List<GetAllCategoriesQueryResponseModel> GetExamples() =>
        new()
        {
            new GetAllCategoriesQueryResponseModel
            {
                Id = 69,
                Name = "First name"
            },
            new GetAllCategoriesQueryResponseModel
            {
                Id = 420,
                Name = "Second name"
            }
        };
}