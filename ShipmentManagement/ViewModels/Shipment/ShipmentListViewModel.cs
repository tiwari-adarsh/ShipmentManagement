using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;

namespace ShipmentManagement.ViewModels.Shipment
{
    public class ShipmentListViewModel
    {
        public List<Models.Shipment> Shipments { get; set; } = new();
        public int TotalCount { get; set; }
        public int PendingCount { get; set; }
        public int InTransitCount { get; set; }
        public int DeliveredCount { get; set; }
        public int DelayedCount { get; set; }

        // Filters
        public string? SearchTerm { get; set; }
        public string? FilterStatus { get; set; }
        public string? FilterType { get; set; }
    }
}