using System.ComponentModel.DataAnnotations;

namespace BerberSimulasyonu.Models
{
    public class Service
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // "Saç" or "Sakal"
        
        public bool IsActive { get; set; } = true;
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        
        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
