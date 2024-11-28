using Core.Domain.Entities;
using Core.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly IDbContext _context;

    public CartRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId) ?? new Cart { CustomerId = customerId };
    }

    public async Task AddToCartAsync(int customerId, int productId, int quantity)
    {
        var cart = await GetCartByCustomerIdAsync(customerId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
            cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
        else
            item.Quantity += quantity;

        _context.Carts.Update(cart);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(int customerId, int productId)
    {
        var cart = await GetCartByCustomerIdAsync(customerId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            cart.Items.Remove(item);
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UpdateCartAsync(Cart cart)
    {
        try
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log exception here if necessary
            return false;
        }
    }

}
