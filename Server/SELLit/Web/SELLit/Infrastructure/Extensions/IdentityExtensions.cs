using System.Security.Claims;

namespace SELLit.Server.Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static string GetId(this ClaimsPrincipal user)
    {
        //return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
        //    !.Value;
        return user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("No NameIdentifier Claim provided");
    }
}