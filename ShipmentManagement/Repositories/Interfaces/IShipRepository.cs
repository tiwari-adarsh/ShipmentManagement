using ShipmentManagement.Models;

namespace ShipmentManagement.Repositories.Interfaces
{
    public interface IShipRepository
    {
        Task<List<Ship>> GetAllAsync();
        Task<Ship?> GetByIdAsync(int id);
        Task AddAsync(Ship ship);
        Task UpdateAsync(Ship ship);
        Task<bool> HasShipmentsAsync(int shipId);
        Task DeleteAsync(int id);
        Task<bool> HasActiveShipmentsAsync(int id);
    }
}