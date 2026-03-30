using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipmentManagement.Models
{
    public class ShipmentStatusLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? CurrentLocation { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [MaxLength(50)]
        public string? UpdatedBy { get; set; }

        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; } = null!;
    }
}