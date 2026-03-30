using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.Models
{
    public class LogHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EntityName { get; set; } // e.g., "Ship", "Customer", "Shipment"

        [Required]
        public string Action { get; set; } // e.g., "Create", "Update", "Delete"

        [Required]
        public string EntityId { get; set; } // The ID of the record being changed

        public string? Details { get; set; } // e.g., "Changed capacity from 500 to 600"

        [Required]
        public string PerformedBy { get; set; } // User Email from Session

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}