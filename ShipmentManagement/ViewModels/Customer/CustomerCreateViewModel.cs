using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.ViewModels.Customer
{
    public class CustomerCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer code is required")]
        [Display(Name = "Customer Code")]
        [MaxLength(10)]
        public string CustomerCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer name is required")]
        [Display(Name = "Customer Name")]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name")]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [Display(Name = "Email")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [Display(Name = "Phone")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "City")]
        [MaxLength(50)]
        public string? City { get; set; }

        [Display(Name = "State / Country")]
        [MaxLength(50)]
        public string? StateCountry { get; set; }
    }
}