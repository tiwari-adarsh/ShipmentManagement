using ShipmentManagement.ViewModels.Shipment;

namespace ShipmentManagement.Services.Interfaces
{
    public interface IShipmentService
    {
        Task<ShipmentCreateViewModel?> GetCreateViewModelAsync();
        Task<bool> CreateShipmentAsync(ShipmentCreateViewModel model);
        Task<ShipmentListViewModel> GetAllAsync(string? search, string? status, string? type);
        Task<ShipmentDetailsViewModel?> GetDetailsAsync(int id);
        Task<ShipmentStatusUpdateViewModel?> GetStatusUpdateViewModelAsync(int id);
        Task<bool> UpdateStatusAsync(ShipmentStatusUpdateViewModel model);
        // Auto-delay
        Task AutoMarkDelayedAsync();
    }
}