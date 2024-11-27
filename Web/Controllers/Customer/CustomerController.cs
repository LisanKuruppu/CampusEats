using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence.Repositories;
using Core.Domain.Entities;
using Core.Application.Interfaces;


[Route("api/customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IUserService _userService;

    public CustomerController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        await _userService.RegisterUserAsync(dto);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var user = await _userService.LoginAsync(dto);
        return Ok(new { user.Id, user.Name, user.Email, user.Role });
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(int userId, UpdateProfileDto dto)
    {
        await _userService.UpdateProfileAsync(userId, dto);
        return Ok("Profile updated successfully.");
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts([FromServices] IProductRepository productRepository)
    {
        var products = await productRepository.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("cart")]
    public async Task<IActionResult> GetCart([FromServices] ICartRepository cartRepository, int customerId)
    {
        var cart = await cartRepository.GetCartByCustomerIdAsync(customerId);
        return Ok(cart);
    }

    [HttpPost("cart/add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        if (dto == null || dto.CustomerId <= 0 || dto.ProductId <= 0 || dto.Quantity <= 0)
            return BadRequest("Invalid cart data.");

        var result = await _cartService.AddToCartAsync(dto.CustomerId, dto.ProductId, dto.Quantity);

        if (!result)
            return StatusCode(500, "Failed to add item to cart.");

        return Ok("Item added to cart.");
    }

    
    [HttpPost("cart/remove")]
    public async Task<IActionResult> RemoveFromCart([FromServices] ICartRepository cartRepository, int customerId, int productId)
    {
        await cartRepository.RemoveFromCartAsync(customerId, productId);
        return Ok("Item removed from cart.");
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromServices] IOrderRepository orderRepository, [FromServices] ICartRepository cartRepository, int customerId)
    {
        var cart = await cartRepository.GetCartByCustomerIdAsync(customerId);
        if (!cart.Items.Any())
            return BadRequest("Cart is empty.");

        var order = new Order
        {
            CustomerId = customerId,
            Items = cart.Items
        };

        await orderRepository.PlaceOrderAsync(order);
        return Ok("Order placed successfully.");
    }

}
