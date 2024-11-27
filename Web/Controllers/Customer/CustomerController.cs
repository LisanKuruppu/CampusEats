using Core.Application.DTOs;
using Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/customer")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICartService _cartService;
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;

    public CustomerController(
        IUserService userService, 
        ICartService cartService,
        IProductRepository productRepository,
        ICartRepository cartRepository,
        IOrderRepository orderRepository)
    {
        _userService = userService;
        _cartService = cartService;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
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
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productRepository.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("cart")]
    public async Task<IActionResult> GetCart(int customerId)
    {
        var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
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
    public async Task<IActionResult> RemoveFromCart(int customerId, int productId)
    {
        await _cartRepository.RemoveFromCartAsync(customerId, productId);
        return Ok("Item removed from cart.");
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(int customerId)
    {
        var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
        if (cart == null || !cart.Items.Any())
            return BadRequest("Cart is empty.");

        var order = new Order
        {
            CustomerId = customerId,
            Items = cart.Items
        };

        await _orderRepository.PlaceOrderAsync(order);
        return Ok("Order placed successfully.");
    }
}
