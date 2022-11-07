using Microsoft.AspNetCore.Identity;

namespace SELLit.Data.Models;

public sealed class User : IdentityUser
{
    private HashSet<Product> _products = new();

    public User(string firstName, string lastName, string email, string userName)
    {
        this.FirstName = GuardWith.NotNull(firstName);
        this.LastName = GuardWith.NotNull(lastName);
        this.Email = GuardWith.NotNull(email);
        this.UserName = GuardWith.NotNull(userName);
    }
    
    private User() {}

    public string FirstName { get; private set; } = "Unknown";

    public string LastName { get; private set; } = "Unknown";
    
    public IReadOnlyCollection<Product> Products => _products;
}