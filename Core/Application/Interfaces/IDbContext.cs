using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Interfaces
{
    public interface IDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Product> Products { get; }
        DbSet<Cart> Carts { get; }
        DbSet<Order> Orders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
