using Core.Application.DTOs;
using Core.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

}
