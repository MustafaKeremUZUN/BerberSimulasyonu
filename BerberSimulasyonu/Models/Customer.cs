using System.ComponentModel.DataAnnotations;

namespace BerberSimulasyonu.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public int VisitCount { get; set; } = 0;
        
        // Navigation property for appointments
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        
        public string FullName => $"{FirstName} {LastName}";
    }
}
