using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipmentManagement.Models
{
    public class ShipmentTracking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ShipmentId { get; set; }

        [Required, MaxLength(100)]
        public string StepTitle { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? StepLocation { get; set; }

        public DateTime StepDate { get; set; }

        // done / current / pending
        [MaxLength(20)]
        public string StepStatus { get; set; } = "pending";

        public int StepOrder { get; set; }

        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; } = null!;
    }
}