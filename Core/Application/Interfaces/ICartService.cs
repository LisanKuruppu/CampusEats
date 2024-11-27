using Core.Application.DTOs;
using Core.Domain.Entities;

namespace Core.Application.Interfaces
{
    public interface ICartService
    {
        Task<bool> AddToCartAsync(int customerId, int productId, int quantity);
    }
}
