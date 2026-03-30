using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.Models
{
    public class Port
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string PortCode { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string PortName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}