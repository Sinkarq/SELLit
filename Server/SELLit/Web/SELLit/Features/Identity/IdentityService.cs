using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace SELLit.Server.Features.Identity;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<User> userManager;

    public IdentityService(UserManager<User> userManager) => this.userManager = userManager;

    public string GenerateJwtToken(
        string userId,
        string username,
        string secret,
        IList<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        
        var claimsArray = new List<Claim>();

        claimsArray.AddRange(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
            });

        claimsArray.AddRange(
            roles.Select(
                role =>
                    new Claim(ClaimTypes.Role, role)));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claimsArray),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> Username(string userId)
        => await this.userManager.Users.Where(x => x.Id == userId).Select(x => x.UserName).FirstOrDefaultAsync();
}