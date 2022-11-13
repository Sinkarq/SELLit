namespace SELLit.Data.Models;

public sealed class Product : BaseDeletableModel<int>
{
    public required string Title { get; set; } = "Unknown";

    public required string Description { get; set; } = "Unknown";

    public required string Location { get; set; } = "Unknown";

    public required string PhoneNumber { get; set; } = "Unknown";

    public required double Price { get; set; }
    
    public required int OrderCount { get; set; }
    
    public required DeliveryResponsibility DeliveryResponsibility { get; set; }

    public required string UserId { get; set; } = "Unknown";
    public User User { get; set; } = default!;

    public required int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}