using System.ComponentModel.DataAnnotations;

namespace BerberSimulasyonu.Models
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Question { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string OptionA { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string OptionB { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string OptionC { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string OptionD { get; set; } = string.Empty;
        
        [Required]
        public string CorrectOption { get; set; } = "A"; // A, B, C, veya D
        
        [StringLength(300)]
        public string? Explanation { get; set; } // Doğru cevabın açıklaması
        
        public int Difficulty { get; set; } = 1; // 1: Kolay, 2: Orta, 3: Zor
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public string? Category { get; set; } // Soru kategorisi (örn: "Saç Kesimi", "Sakal", "Hijyen")
    }
}
