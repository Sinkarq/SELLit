using Microsoft.AspNetCore.Mvc.Filters;
using SELLit.Server.Features.Identity;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Infrastructure.Filters;

public class RequestLoggerFilter : ActionFilterAttribute
{
    private readonly Serilog.ILogger logger;
    private readonly ICurrentUser currentUser;
    private readonly IIdentityService identityService;

    public RequestLoggerFilter(Serilog.ILogger logger, ICurrentUser currentUser, IIdentityService identityService)
    {
        this.logger = logger;
        this.currentUser = currentUser;
        this.identityService = identityService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controller = context.Controller.ToString();
        var action = context.ActionDescriptor.DisplayName;
        var userId = this.currentUser.UserId ?? "Anonymous-Id";
        var userName = await this.identityService.Username(userId) ?? "Anonymous";

        this.logger.Information(
            "SELLit Request: {Name}, {@UserId} {@UserName} {Action}",
            controller,
            userId,
            userName,
            action);
        
        await next();
    }
}