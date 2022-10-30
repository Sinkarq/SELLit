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
        DeliveryResponsibility deliveryResponsibility)
    {
        NullGuardMethods.Guard(title, description, location, phoneNumber, userId);
        NullGuardMethods.Guard(price);
        NullGuardMethods.Guard(categoryId);
        NullGuardMethods.Guard(deliveryResponsibility);
        this.Title = title;
        this.Description = description;
        this.Location = location;
        this.PhoneNumber = phoneNumber;
        this.Price = price;
        this.CategoryId = categoryId;
        this.UserId = userId;
        this.OrderCount = 0;
    }

    private Product() {}
    
    public string Title { get; private set; }

    public string Description { get; private set; }
    
    public string Location { get; private set; }
    
    public string PhoneNumber { get; private set; }

    public double Price { get; private set; }
    
    public int OrderCount { get; private set; }
    
    public DeliveryResponsibility DeliveryResponsibility { get; private set; }

    public string UserId { get; set; }
    public User User { get; private set; }
    
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }

    public void Update(string title, string description, string location, string phoneNumber, double price,
        DeliveryResponsibility deliveryResponsibility)
    {
        NullGuardMethods.Guard(title, description, location, phoneNumber);
        NullGuardMethods.Guard(price);
        NullGuardMethods.Guard(deliveryResponsibility);

        this.Title = title;
        this.Description = description;
        this.Location = location;
        this.PhoneNumber = phoneNumber;
        this.Price = price;
        this.DeliveryResponsibility = deliveryResponsibility;
    }
}