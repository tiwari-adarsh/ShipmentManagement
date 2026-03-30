using ShipmentManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShipmentManagement.Models
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string ShipmentCode { get; set; } = string.Empty;  

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        public ShipmentType ShipmentType { get; set; }

        public CargoType CargoType { get; set; }

        [Required]
        public decimal WeightMT { get; set; }

        [MaxLength(30)]
        public string? ContainerNumber { get; set; }

        [Required]
        public int ShipId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required, MaxLength(100)]
        public string SourcePort { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DestinationPort { get; set; } = string.Empty;

        [Required]
        public DateTime DepartureDate { get; set; }

        public DateTime? ArrivalDate { get; set; }         
        public DateTime? ActualArrivalDate { get; set; }  

        [MaxLength(500)]
        public string? SpecialInstructions { get; set; }

        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("ShipId")]
        public Ship Ship { get; set; } = null!;

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } = null!;

        public ICollection<ShipmentStatusLog> StatusLogs { get; set; } = new List<ShipmentStatusLog>();
        public ICollection<ShipmentTracking> TrackingSteps { get; set; } = new List<ShipmentTracking>();
    }
}