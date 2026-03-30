using ShipmentManagement.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.Models
{
    public class Ship
    {
        [Key]
       public int Id { get; set; }

       [Required, MaxLength(10)]
       public string ShipCode { get; set; } = string.Empty;   // S001, S002

       [Required, MaxLength(100)]
       public string ShipName { get; set; } = string.Empty;

       [Required, MaxLength(20)]
       public string ImoNumber { get; set; } = string.Empty;

       [Required]
       public decimal CapacityMT { get; set; }

       [Required, MaxLength(50)]
       public string ShipType { get; set; } = string.Empty;

       [Required, MaxLength(50)]
       public string FlagCountry { get; set; } = string.Empty;

       [Required]
       public int YearBuilt { get; set; }

       public ShipStatus Status { get; set; } = ShipStatus.Active;

       public DateTime CreatedAt { get; set; } = DateTime.Now;
       public DateTime? UpdatedAt { get; set; }

       public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
