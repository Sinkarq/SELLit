using Microsoft.AspNetCore.Identity;

namespace SELLit.Data.Models;

public sealed class User : IdentityUser
{
    private HashSet<Product> _products = new();

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public override required string? Email { get; set; }

    public override required string? UserName { get; set; }

    public IReadOnlyCollection<Product> Products => _products;
}