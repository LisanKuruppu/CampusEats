using Core.Domain.Entities;
using Core.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public interface IOrderRepository
{
    Task PlaceOrderAsync(Order order);
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
}

public class OrderRepository : IOrderRepository
{
    private readonly IDbContext _context;

    public OrderRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task PlaceOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }
}
