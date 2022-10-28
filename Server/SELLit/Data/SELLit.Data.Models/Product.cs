using SELLit.Common;

namespace SELLit.Data.Models;

public sealed class Product : BaseDeletableModel<int>
{
    public Product(
        string title, 
        string description, 
        string location, 
        string phoneNumber, 
        double price, 
        int categoryId,
        string userId,
        int orderCount)
    {
        NullGuardMethods.Guard(title, description, location, phoneNumber, userId);
        NullGuardMethods.Guard(price);
        NullGuardMethods.Guard(categoryId, orderCount);
        this.Title = title;
        this.Description = description;
        this.Location = location;
        this.PhoneNumber = phoneNumber;
        this.Price = price;
        this.CategoryId = categoryId;
        this.UserId = userId;
        this.OrderCount = orderCount;
    }

    private Product() {}
    
    public string Title { get; private set; }

    public string Description { get; private set; }
    
    public string Location { get; private set; }
    
    public string PhoneNumber { get; private set; }

    public double Price { get; private set; }
    
    public int OrderCount { get; private set; }

    public string UserId { get; set; }
    public User User { get; private set; }
    
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
}