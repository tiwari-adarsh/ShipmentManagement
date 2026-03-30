using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.ViewModels.Ship
{
    public class ShipCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ship code is required")]
        [Display(Name = "Ship Code")]
        [MaxLength(10)]
        public string ShipCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ship name is required")]
        [Display(Name = "Ship Name")]
        [MaxLength(100)]
        public string ShipName { get; set; } = string.Empty;

        [Required(ErrorMessage = "IMO number is required")]
        [Display(Name = "IMO Number")]
        [MaxLength(20)]
        public string ImoNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 9999999, ErrorMessage = "Capacity must be greater than 0")]
        [Display(Name = "Capacity (MT)")]
        public decimal CapacityMT { get; set; }

        [Required(ErrorMessage = "Ship type is required")]
        [Display(Name = "Ship Type")]
        public string ShipType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Flag country is required")]
        [Display(Name = "Flag Country")]
        public string FlagCountry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year built is required")]
        [Range(1900, 2100, ErrorMessage = "Enter a valid year")]
        [Display(Name = "Year Built")]
        public int YearBuilt { get; set; } = DateTime.Now.Year;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Active";

        // Dropdowns
        public List<SelectListItem> ShipTypes { get; set; } = new()
        {
            new("Bulk Carrier","Bulk Carrier"),
            new("Container","Container"),
            new("Tanker","Tanker"),
            new("Cargo","Cargo"),
            new("RoRo","RoRo"),
        };

        public List<SelectListItem> StatusList { get; set; } = new()
        {
            new("Active","Active"),
            new("Inactive","Inactive"),
            new("Maintenance","Maintenance"),
        };

        public List<SelectListItem> Countries { get; set; } = new()
        {
            new("India","India"),
            new("Singapore","Singapore"),
            new("Panama","Panama"),
            new("Liberia","Liberia"),
            new("UK","UK"),
            new("USA","USA"),
            new("UAE","UAE"),
        };
    }
}