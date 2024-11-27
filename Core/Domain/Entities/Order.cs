namespace Core.Domain.Entities;
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public List<CartItem> Items { get; set; } = new();
    public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
}
