using System.ComponentModel.DataAnnotations;

namespace BerberSimulasyonu.Models
{
    public class EmployeeApplication
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
        
        [StringLength(500)]
        public string? Experience { get; set; } // Deneyim bilgisi
        
        [StringLength(100)]
        public string? DesiredPosition { get; set; } = "Berber"; // İstenen pozisyon
        
        public decimal DesiredHourlyRate { get; set; } = 0; // İstenen saatlik ücret
        
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        
        // Quiz sonuçları
        public int CorrectAnswers { get; set; } = 0; // Doğru cevap sayısı
        public int TotalQuestions { get; set; } = 5; // Toplam soru sayısı
        public decimal QuizScore { get; set; } = 0; // Quiz puanı (0-100)
        
        // Başvuru durumu
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        
        public DateTime? ReviewedDate { get; set; }
        
        [StringLength(500)]
        public string? ReviewNotes { get; set; } // Değerlendirme notları
        
        // Quiz cevapları (JSON formatında saklanacak)
        [StringLength(2000)]
        public string? QuizAnswers { get; set; }
        
        // Navigation properties
        public virtual Employee? Employee { get; set; } // Onaylanırsa oluşturulan çalışan
        public int? EmployeeId { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
    
    public enum ApplicationStatus
    {
        Pending = 0,    // Beklemede
        Approved = 1,   // Onaylandı
        Rejected = 2,   // Reddedildi
        Withdrawn = 3   // Çekildi
    }
}
