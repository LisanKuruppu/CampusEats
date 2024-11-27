using Core.Application.DTOs;
using Core.Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<User> LoginAsync(LoginUserDto dto);
    }

    public class UserService : IUserService
    {
        private readonly CampusEatsDbContext _context;

        public UserService(CampusEatsDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Hash the password
                Role = "Customer"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return user;
        }

        public async Task UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.Name = dto.Name;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
        }

    }
}
