using Core.Application.DTOs;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Application.Interfaces;
using BCrypt.Net;

namespace Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContext _context;

        public UserService(IDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> LoginAsync(LoginUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

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
