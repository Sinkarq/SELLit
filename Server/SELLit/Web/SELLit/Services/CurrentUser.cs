using System.Security.Claims;
using CommunityToolkit.Diagnostics;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Services;

public sealed class CurrentUser : ICurrentUser
{
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        Guard.IsNotNull(httpContext, $"{nameof(httpContext)} returned null http context");
        this.UserId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public string? UserId { get; }
}