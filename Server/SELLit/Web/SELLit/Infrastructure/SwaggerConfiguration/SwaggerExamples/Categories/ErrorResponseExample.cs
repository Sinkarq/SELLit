using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples.Categories;

internal sealed class ErrorResponseExample : IExamplesProvider<ErrorResponse>
{
    public ErrorResponse GetExamples() =>
        new()
        {
            Errors = new List<ErrorResponseModel>
            {
                new(message: "Example message"),
                new() {FieldName = "Example field", Message = "Example message"}
            }
        };
}