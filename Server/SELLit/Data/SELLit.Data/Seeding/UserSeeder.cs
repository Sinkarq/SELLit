using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SELLit.Common;

namespace SELLit.Data.Seeding;

public sealed class UserSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        if (await userManager.FindByEmailAsync("sinanilhanov@gmail.com") == null)
        {
            var adminUser = new User("Sinan", "Abdulgafurov", "sinanilhanov@gmail.com", "Sinkarq");
            await userManager.CreateAsync(adminUser, "password1234");
            await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
        }
        

        if (await userManager.FindByEmailAsync("john@gmail.com") == null)
        {
            var user = new User("John", "Sashev", "john@gmail.com", "John");
            await userManager.CreateAsync(user, "password1234");
        }
        
    }
}