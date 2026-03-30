using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;

namespace ShipmentManagement.ViewModels.Ship
{
    public class ShipListViewModel
    {
        public List<Models.Ship> Ships { get; set; } = new();
        public int TotalShips { get; set; }
        public int ActiveShips { get; set; }
        public int MaintenanceShips { get; set; }
        public int InactiveShips { get; set; }
        public string? SearchTerm { get; set; }
        public string? FilterType { get; set; }
    }
}