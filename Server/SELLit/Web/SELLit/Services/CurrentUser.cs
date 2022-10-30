using System.Security.Claims;
using SELLit.Server.Services.Interfaces;

namespace SELLit.Server.Services;

public sealed class CurrentUser : ICurrentUser
{
    public CurrentUser(IHttpContextAccessor httpContextAccessor)
        => this.UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string UserId { get; }
}