namespace SELLit.Server.Features.Identity;

public interface IIdentityService
{
    string GenerateJwtToken(string userId,string username, string secret, IList<string> roles);

    Task<string?> Username(string userId);
}