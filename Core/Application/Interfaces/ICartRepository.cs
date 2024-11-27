using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByCustomerIdAsync(int customerId);
        Task<bool> UpdateCartAsync(Cart cart);
    }
}
