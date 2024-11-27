namespace Core.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
