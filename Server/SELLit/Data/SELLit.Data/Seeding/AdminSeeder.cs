using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Common;

namespace SELLit.Data.Seeding;

public sealed class AdminSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        if (await userManager.FindByEmailAsync("sinanilhanov@gmail.com") != null)
        {
            return;
        };
        var user = new User("Sinan", "Abdulgafurov", "sinanilhanov@gmail.com", "Sinkarq");
        await userManager.CreateAsync(user, "pfcludogorets1945");
        await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
    }
}