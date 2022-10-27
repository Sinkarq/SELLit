using SELLit.Common;

namespace SELLit.Data.Models;

public sealed class Category
{
    private readonly HashSet<Product> _products = new();
    

    public Category(string name)
    {
        NullGuardMethods.Guard(name);
        this.Name = name;
    }
    
    private Category() {}

    public string Name { get; private set; }

    public IReadOnlyCollection<Product> Products => _products;
}