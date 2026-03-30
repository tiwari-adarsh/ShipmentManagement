using ShipmentManagement.ViewModels.Dashboard;

namespace ShipmentManagement.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel?> GetDashboardDataAsync();
    }
}