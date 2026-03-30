using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;

namespace ShipmentManagement.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardStat> GetStatsAsync();
        Task<List<(string Month, int Count, int Delivered)>> GetMonthlyTrendAsync();
        Task<List<(ShipmentStatus Status, int Count)>> GetStatusDistributionAsync();
        Task<List<(string ShipName, int Count)>> GetShipWiseCountAsync();
    }
}