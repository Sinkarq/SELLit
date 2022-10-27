namespace SELLit.Server.Features.Identity;

public interface IIdentityService
{
    string GenerateJwtToken(string userId,string username, string secret);

    Task<string> Username(string userId);
}