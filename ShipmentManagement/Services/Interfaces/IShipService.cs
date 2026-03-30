using ShipmentManagement.ViewModels.Ship;

namespace ShipmentManagement.Services.Interfaces
{
    public interface IShipService
    {
        Task<ShipListViewModel> GetAllAsync(string? search, string? filterType);
        Task<ShipCreateViewModel> GetCreateViewModelAsync();
        Task<ShipCreateViewModel?> GetEditViewModelAsync(int id);
        Task<bool> CreateAsync(ShipCreateViewModel model);
        Task<bool> UpdateAsync(ShipCreateViewModel model);
        Task<bool> DeleteAsync(int id);

        Task<bool> HasActiveShipmentsAsync(int id);
    }
}
