using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SELLit.Server.Infrastructure.Filters;


/// <summary>
/// Converts the result in NotFoundResult if the returning value is null
/// </summary>
public sealed class ModelOrNotFoundActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult {Value: null})
        {
            context.Result = new NotFoundResult();
        }
    }
}