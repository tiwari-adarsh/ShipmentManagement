using System.ComponentModel.DataAnnotations;

namespace ShipmentManagement.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Role { get; set; } = "Admin";

        public bool IsActive { get; set; } = true;

        public DateTime? LastLogin { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}