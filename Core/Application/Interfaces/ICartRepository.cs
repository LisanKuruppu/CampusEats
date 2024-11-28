using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByCustomerIdAsync(int customerId);
        Task AddToCartAsync(int customerId, int productId, int quantity);
        Task RemoveFromCartAsync(int customerId, int productId);
        Task<bool> UpdateCartAsync(Cart cart); // Add this method
    }
}
