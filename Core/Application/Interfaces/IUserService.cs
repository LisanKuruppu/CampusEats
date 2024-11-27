using Core.Application.DTOs;
using Core.Domain.Entities;


namespace Core.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<User?> LoginAsync(LoginUserDto dto);
        Task UpdateProfileAsync(int userId, UpdateProfileDto dto);
        
    }
}
