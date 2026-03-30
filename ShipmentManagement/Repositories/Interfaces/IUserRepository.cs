using ShipmentManagement.Models;

namespace ShipmentManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> ValidateUserAsync(string email, string password);
        Task UpdateLastLoginAsync(int userId);
    }
}