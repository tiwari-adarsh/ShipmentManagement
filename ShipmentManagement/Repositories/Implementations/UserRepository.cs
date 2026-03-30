using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Data;
using ShipmentManagement.Models;
using ShipmentManagement.Repositories.Interfaces;

namespace ShipmentManagement.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == email && u.IsActive);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // In production: use BCrypt.Verify(password, user.PasswordHash)
            // For this assessment: plain text comparison
            return await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == email &&
                    u.PasswordHash == password &&
                    u.IsActive);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}