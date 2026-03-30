using ShipmentManagement.Repositories.Interfaces;
using ShipmentManagement.Services.Interfaces;
using ShipmentManagement.ViewModels.Dashboard;

namespace ShipmentManagement.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        private readonly ILogActionService _logActionService;

        public DashboardService(IDashboardRepository repo, ILogActionService logActionService)
        {
            _repo = repo;
            _logActionService = logActionService;
        }

        public async Task<DashboardViewModel?> GetDashboardDataAsync()
        {
            try
            {
            var stats = await _repo.GetStatsAsync();
            var monthly = await _repo.GetMonthlyTrendAsync();
            var statDist = await _repo.GetStatusDistributionAsync();
            var shipWise = await _repo.GetShipWiseCountAsync();

            return new DashboardViewModel
            {
                TotalShipments = stats.TotalShipments,
                Upcoming = stats.Upcoming,
                Ongoing = stats.Ongoing,
                Delivered = stats.Delivered,
                Delayed = stats.Delayed,

                MonthlyTrend = monthly.Select(m => new MonthlyShipmentData
                {
                    Month = m.Month,
                    Shipments = m.Count,
                    Delivered = m.Delivered
                }).ToList(),

                StatusData = statDist.Select(s => new StatusDistribution
                {
                    Status = s.Status,
                    Count = s.Count
                }).ToList(),

                ShipWiseData = shipWise.Select(s => new ShipWiseData
                {
                    ShipName = s.ShipName,
                    Count = s.Count
                }).ToList(),

                RecentActivities = new List<ActivityItem>
                {
                    new() { Message = "Latest shipment delivered at port", TimeAgo = "2 hours ago",   Color = "#2ecc8a" },
                    new() { Message = "Shipment delayed — weather alert",  TimeAgo = "4 hours ago",   Color = "#e85656" },
                    new() { Message = "New booking created successfully",  TimeAgo = "6 hours ago",   Color = "#f0a500" },
                    new() { Message = "Vessel departed from source port",  TimeAgo = "Yesterday",     Color = "#00c8e0" },
                }
            };
            }
            catch (Exception ex)
            {
                await _logActionService.LogActionAsync("DashboardService.GetDashboardDataAsync", "GET", "", $"Error while Binding Dashboard Data : {ex.Message}");
                return null;
            }
        }
    }
}
