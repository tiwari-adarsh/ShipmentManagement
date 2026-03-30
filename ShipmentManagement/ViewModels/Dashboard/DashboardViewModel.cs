using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;

namespace ShipmentManagement.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalShipments { get; set; }
        public int Upcoming { get; set; }
        public int Ongoing { get; set; }
        public int Delivered { get; set; }
        public int Delayed { get; set; }

        public List<MonthlyShipmentData> MonthlyTrend { get; set; } = new();
        public List<StatusDistribution> StatusData { get; set; } = new();
        public List<ShipWiseData> ShipWiseData { get; set; } = new();
        public List<ActivityItem> RecentActivities { get; set; } = new();
    }

    public class MonthlyShipmentData
    {
        public string Month { get; set; } = string.Empty;
        public int Shipments { get; set; }
        public int Delivered { get; set; }
    }

    public class StatusDistribution
    {
        public ShipmentStatus Status { get; set; }
        public int Count { get; set; }
    }

    public class ShipWiseData
    {
        public string ShipName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ActivityItem
    {
        public string Message { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
        public string Color { get; set; } = "#2ecc8a"; // green/amber/red
    }
}