using ShipmentManagement.Models;

namespace ShipmentManagement.Repositories.Interfaces
{
    public interface IShipmentRepository
    {
        // Core CRUD
        Task<List<Shipment>> GetAllAsync();
        Task<List<Shipment>> GetFilteredAsync(string? search, string? status, string? type);
        Task<Shipment?> GetByIdAsync(int id);
        Task<Shipment?> GetByCodeAsync(string code);
        Task AddAsync(Shipment shipment);
        Task UpdateAsync(Shipment shipment);
        Task DeleteAsync(int id);

        // Stats
        Task<int> GetCountByStatusAsync(string status);
        Task<int> GetTotalCountAsync();

        // Auto-delay
        Task<List<Shipment>> GetOverdueShipmentsAsync();

        Task<string> GenerateShipmentCodeAsync();
        Task AddStatusLogAsync(ShipmentStatusLog log);
        Task AddTrackingStepAsync(ShipmentTracking step);

        Task<List<Shipment>> GetByStatusAsync(string status);
        Task<Shipment?> GetByIdWithDetailsAsync(int id);
    }
}