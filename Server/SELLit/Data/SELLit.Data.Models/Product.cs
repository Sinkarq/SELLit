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
        this.Title = GuardWith.NotNull(title);
        this.Description = GuardWith.NotNull(description);
        this.Location = GuardWith.NotNull(location);
        this.PhoneNumber = GuardWith.NotNull(phoneNumber);
        this.Price = GuardWith.NotNull(price);
        this.CategoryId = GuardWith.NotNull(categoryId);
        this.UserId = GuardWith.NotNull(userId);
        this.DeliveryResponsibility = GuardWith.NotNull(deliveryResponsibility);
        this.OrderCount = 0;
    }

    private Product() {}

    public string Title { get; private set; } = "Unknown";

    public string Description { get; private set; } = "Unknown";

    public string Location { get; private set; } = "Unknown";

    public string PhoneNumber { get; private set; } = "Unknown";

    public double Price { get; private set; }
    
    public int OrderCount { get; private set; }
    
    public DeliveryResponsibility DeliveryResponsibility { get; private set; }

    public string UserId { get; set; } = "Unknown";
    public User User { get; private set; } = default!;

    public int CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;

    public void Update(string title, string description, string location, string phoneNumber, double price,
        DeliveryResponsibility deliveryResponsibility)
    {
        this.Title = GuardWith.NotNull(title);
        this.Description = GuardWith.NotNull(description);
        this.Location = GuardWith.NotNull(location);
        this.PhoneNumber = GuardWith.NotNull(phoneNumber);
        this.Price = GuardWith.NotNull(price);
        this.DeliveryResponsibility = GuardWith.NotNull(deliveryResponsibility);
    }
}