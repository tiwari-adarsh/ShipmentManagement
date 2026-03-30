using ShipmentManagement.Models;

namespace ShipmentManagement.ViewModels.Shipment
{
    public class ShipmentDetailsViewModel
    {
        public ShipmentManagement.Models.Shipment Shipment { get; set; } = null!;
        public List<ShipmentTracking> TrackingSteps { get; set; } = new();
        public List<ShipmentStatusLog> StatusLogs { get; set; } = new();
    }
}