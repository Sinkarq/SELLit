using Swashbuckle.AspNetCore.Filters;

namespace SELLit.Server.Infrastructure.SwaggerConfiguration.SwaggerExamples;

public class ErrorModelExample : IExamplesProvider<ErrorModel>
{
    public ErrorModel GetExamples() => new(new[] {"example error"}, 404);
}