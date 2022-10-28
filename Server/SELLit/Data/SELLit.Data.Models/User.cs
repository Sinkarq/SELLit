using Microsoft.AspNetCore.Identity;
using SELLit.Common;

namespace SELLit.Data.Models;

public sealed class User : IdentityUser
{
    private HashSet<Product> _products = new();

    public User(string firstName, string lastName, string email, string userName)
    {
        NullGuardMethods.Guard(firstName, lastName, email, userName);
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.UserName = userName;
    }
    
    private User() {}
    
    public string FirstName { get; private set; }

    public string LastName { get; private set; }
    
    public IReadOnlyCollection<Product> Products => _products;
}