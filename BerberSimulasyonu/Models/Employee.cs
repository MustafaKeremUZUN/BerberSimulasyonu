using System.ComponentModel.DataAnnotations;

namespace BerberSimulasyonu.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }
        
        [StringLength(100)]
        public string? Position { get; set; } = "Berber"; // Berber, Usta Berber, vb.
        
        public decimal HourlyRate { get; set; } = 0; // Saatlik ücret
        
        public bool IsActive { get; set; } = true;
        
        public DateTime HireDate { get; set; } = DateTime.Now;
        
        public DateTime? WorkStartTime { get; set; } // İşe başlama saati (bugün için)
        public DateTime? WorkEndTime { get; set; } // İşten çıkış saati (bugün için)
        
        // Çalışma saatleri (genel)
        public TimeSpan DefaultWorkStart { get; set; } = new TimeSpan(9, 0, 0); // 09:00
        public TimeSpan DefaultWorkEnd { get; set; } = new TimeSpan(18, 0, 0); // 18:00
        
        // Navigation properties
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        
        public string FullName => $"{FirstName} {LastName}";
    }
}
