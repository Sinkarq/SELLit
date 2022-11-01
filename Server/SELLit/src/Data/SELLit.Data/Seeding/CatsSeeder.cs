using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SELLit.Data.Seeding;

public class CatsSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var user = new User()
        {
            UserName = "SeededUser",
            Email = "SeededUser@gmail.com"
        };
        if (dbContext.Users.Any())
        {
            return;
        }
        
        await userManager.CreateAsync(user, "PasswordIsGood");

        dbContext.Cats.AddRange(new List<Cat>()
        {
            new("FirstCat", user.Id),
            new("FirstCat", user.Id),
        });

        await dbContext.SaveChangesAsync();
    }
}