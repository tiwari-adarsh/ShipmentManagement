using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.Models
{
    public class Customer
    {
       [Key]
       public int Id { get; set; }
       
       [Required, MaxLength(10)]
       public string CustomerCode { get; set; } = string.Empty;   // C001, C002
       
       [Required, MaxLength(100)]
       public string CustomerName { get; set; } = string.Empty;
       
       [Required, MaxLength(100)]
       public string CompanyName { get; set; } = string.Empty;
       
       [Required, MaxLength(100)]
       public string Email { get; set; } = string.Empty;
       
       [Required, MaxLength(20)]
       public string Phone { get; set; } = string.Empty;
       
       [MaxLength(255)]
       public string? Address { get; set; }
       
       [MaxLength(50)]
       public string? City { get; set; }
       
       [MaxLength(50)]
       public string? StateCountry { get; set; }
       
       public DateTime CreatedAt { get; set; } = DateTime.Now;
       public DateTime? UpdatedAt { get; set; }
       
       public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
