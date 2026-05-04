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
        public int EmployeeId { get; set; }
        
        [Required]
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal OriginalPrice { get; set; }
        
        [Range(0, 100)]
        public decimal DiscountPercentage { get; set; } = 0;
        
        public decimal DiscountAmount { get; set; } = 0;
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal FinalPrice { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        public DateTime? CompletedDate { get; set; }
        
        // Navigation properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
    }
}
