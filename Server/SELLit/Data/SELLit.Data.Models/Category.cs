using CommunityToolkit.Diagnostics;

namespace SELLit.Data.Models;

public sealed class Category : BaseDeletableModel<int>
{
    private readonly HashSet<Product> _products = new();

    public required string Name { get; set; }

    public IReadOnlyCollection<Product> Products => _products;

    public void Update(string name)
    {
        Guard.IsNotNullOrEmpty(name);
        Guard.IsNotNullOrWhiteSpace(name);
        this.Name = name;
    }
}