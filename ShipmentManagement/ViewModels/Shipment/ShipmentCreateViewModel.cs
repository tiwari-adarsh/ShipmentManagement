using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.ViewModels.Shipment
{
    public class ShipmentCreateViewModel
    {
        // Auto-generated — display only
        public string ShipmentCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Booking date is required")]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Shipment type is required")]
        [Display(Name = "Shipment Type")]
        public string ShipmentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cargo type is required")]
        [Display(Name = "Cargo Type")]
        public string CargoType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Weight is required")]
        [Range(0.1, 999999, ErrorMessage = "Weight must be greater than 0")]
        [Display(Name = "Weight (MT)")]
        public decimal WeightMT { get; set; }

        [Display(Name = "Container Number")]
        public string? ContainerNumber { get; set; }

        // Ship
        [Required(ErrorMessage = "Please select a ship")]
        [Display(Name = "Ship")]
        public int ShipId { get; set; }

        // Customer
        [Required(ErrorMessage = "Please select a customer")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        // Route
        [Required(ErrorMessage = "Source port is required")]
        [Display(Name = "Source Port")]
        public string SourcePort { get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination port is required")]
        [Display(Name = "Destination Port")]
        public string DestinationPort { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departure date is required")]
        [Display(Name = "Departure Date")]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Estimated Arrival Date")]
        public DateTime? ArrivalDate { get; set; }

        [Display(Name = "Special Instructions")]
        public string? SpecialInstructions { get; set; }

        // Dropdowns
        public List<SelectListItem> Ships { get; set; } = new();
        public List<SelectListItem> Customers { get; set; } = new();
        public List<SelectListItem> Ports { get; set; } = new();

        public List<SelectListItem> ShipmentTypes { get; set; } = new()
        {
            new("Export", "Export"),
            new("Import", "Import"),
            new("Transshipment", "Transshipment"),
        };

        public List<SelectListItem> CargoTypes { get; set; } = new()
        {
            new("General Cargo", "GeneralCargo"),
            new("Bulk Cargo",    "BulkCargo"),
            new("Liquid Cargo",  "LiquidCargo"),
            new("Refrigerated",  "Refrigerated"),
            new("Hazardous",     "Hazardous"),
            new("Oversized",     "Oversized"),
            new("Container",     "Container"),
        };

        public List<CustomerDropdownItem> CustomerDataList { get; set; } = new();

        public class CustomerDropdownItem
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Company { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
        }
    }
}