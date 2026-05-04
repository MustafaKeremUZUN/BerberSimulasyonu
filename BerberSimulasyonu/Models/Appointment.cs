using System.ComponentModel.DataAnnotations;

namespace BerberSimulasyonu.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        
        [Required]
        public decimal OriginalPrice { get; set; }
        
        public decimal DiscountPercentage { get; set; } = 0;
        
        public decimal DiscountAmount { get; set; } = 0;
        
        [Required]
        public decimal FinalPrice { get; set; }
        
        public string? Notes { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        // Navigation properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
    }
}
