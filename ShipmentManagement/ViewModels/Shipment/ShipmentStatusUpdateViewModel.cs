using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.ViewModels.Shipment
{
    public class ShipmentStatusUpdateViewModel
    {
        public int Id { get; set; }
        public string ShipmentCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string CurrentStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a status")]
        [Display(Name = "New Status")]
        public string NewStatus { get; set; } = string.Empty;

        [Display(Name = "Current Location")]
        [MaxLength(150)]
        public string? CurrentLocation { get; set; }

        [Display(Name = "Remarks")]
        [MaxLength(500)]
        public string? Remarks { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public List<SelectListItem> StatusList { get; set; } = new()
        {
            new("Pending","Pending"),
            new("Loaded","Loaded"),
            new("In Transit","InTransit"),
            new("Port Arrival","PortArrival"),
            new("Customs Clearance","CustomsClearance"),
            new("Delivered","Delivered"),
            new("Delayed","Delayed"),
        };
    }
}