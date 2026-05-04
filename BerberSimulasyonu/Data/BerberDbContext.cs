using Microsoft.EntityFrameworkCore;
using BerberSimulasyonu.Models;

namespace BerberSimulasyonu.Data
{
    public class BerberDbContext : DbContext
    {
        public BerberDbContext(DbContextOptions<BerberDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeApplication> EmployeeApplications { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.VisitCount).HasDefaultValue(0);
            });

            // Service configuration
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasDefaultValue(0);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Appointment configuration
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AppointmentDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.DiscountPercentage).HasDefaultValue(0);
                entity.Property(e => e.DiscountAmount).HasDefaultValue(0);
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
                entity.Property(e => e.Notes).HasMaxLength(500);

                // Foreign key relationships
                entity.HasOne(a => a.Customer)
                      .WithMany(c => c.Appointments)
                      .HasForeignKey(a => a.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Service)
                      .WithMany(s => s.Appointments)
                      .HasForeignKey(a => a.ServiceId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Employee)
                      .WithMany(e => e.Appointments)
                      .HasForeignKey(a => a.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial services
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Klasik Saç Kesimi", Description = "Makine veya makasla standart saç kesimi", Price = 100, Category = "Saç", IsActive = true },
                new Service { Id = 2, Name = "Modern Saç Kesimi", Description = "Trend ve modern saç kesim teknikleri", Price = 150, Category = "Saç", IsActive = true },
                new Service { Id = 3, Name = "Saç Boyama", Description = "Tek renk tam saç boyama hizmeti", Price = 400, Category = "Saç", IsActive = true },
                new Service { Id = 4, Name = "Klasik Sakal Kesimi", Description = "Sakal şekillendirme ve tıraş", Price = 80, Category = "Sakal", IsActive = true },
                new Service { Id = 5, Name = "Sakal Traşı", Description = "Jilet ile profesyonel sakal traşı", Price = 60, Category = "Sakal", IsActive = true },
                new Service { Id = 6, Name = "Sakal Boyama", Description = "Sakal ve bıyık boyama hizmeti", Price = 150, Category = "Sakal", IsActive = true },
                new Service { Id = 7, Name = "Saç ve Sakal Paket", Description = "Saç kesimi + sakal kesimi kombin paketi", Price = 160, Category = "Saç", IsActive = true },
                new Service { Id = 8, Name = "Yüz Masajı", Description = "Rahatlatıcı ve yenileyici yüz masajı", Price = 100, Category = "Sakal", IsActive = true }
            );

            // Employee configuration
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Position).HasMaxLength(100);
                entity.Property(e => e.HireDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // EmployeeApplication configuration
            modelBuilder.Entity<EmployeeApplication>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Experience).HasMaxLength(500);
                entity.Property(e => e.DesiredPosition).HasMaxLength(100);
                entity.Property(e => e.QuizAnswers).HasMaxLength(2000);
                entity.Property(e => e.ReviewNotes).HasMaxLength(500);
                entity.Property(e => e.ApplicationDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Status).HasDefaultValue(ApplicationStatus.Pending);
                
                // Relationship with Employee
                entity.HasOne(e => e.Employee)
                      .WithOne()
                      .HasForeignKey<EmployeeApplication>(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // QuizQuestion configuration
            modelBuilder.Entity<QuizQuestion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Question).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OptionA).IsRequired().HasMaxLength(200);
                entity.Property(e => e.OptionB).IsRequired().HasMaxLength(200);
                entity.Property(e => e.OptionC).IsRequired().HasMaxLength(200);
                entity.Property(e => e.OptionD).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CorrectOption).IsRequired().HasMaxLength(1);
                entity.Property(e => e.Explanation).HasMaxLength(300);
                entity.Property(e => e.Category).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Seed initial quiz questions
            modelBuilder.Entity<QuizQuestion>().HasData(
                new QuizQuestion { Id = 1, Question = "Makine ile saç kesiminde genellikle hangi numara makine başlığı kullanılır?", OptionA = "3-4 numara", OptionB = "1-2 numara", OptionC = "6-8 numara", OptionD = "9-12 numara", CorrectOption = "A", Explanation = "Klasik erkek saç kesiminde genellikle 3-4 numara makine başlığı kullanılır.", Difficulty = 1, Category = "Saç Kesimi" },
                new QuizQuestion { Id = 2, Question = "Sakal traşı öncesi cildi hazırlamak için en iyi yöntem nedir?", OptionA = "Sıcak havlu uygulaması", OptionB = "Soğuk su ile yıkama", OptionC = "Kuru tıraş", OptionD = "Jilet direkt uygulanır", CorrectOption = "A", Explanation = "Sıcak havlu uygulaması sakalları yumuşatır ve cildi tıraşa hazırlar.", Difficulty = 1, Category = "Sakal" },
                new QuizQuestion { Id = 3, Question = "Berber dükkanında hijyen için en önemli kural nedir?", OptionA = "Her müşteri için temiz malzeme kullanmak", OptionB = "Hızlı çalışmak", OptionC = "Pahalı ürünler kullanmak", OptionD = "Dükkanı süslemek", CorrectOption = "A", Explanation = "Hijyen için her müşteri için temiz ve steril malzeme kullanmak esastır.", Difficulty = 1, Category = "Hijyen" },
                new QuizQuestion { Id = 4, Question = "Saç boyama işlemi sonrasında ne kadar beklemek gerekir?", OptionA = "30-45 dakika", OptionB = "5-10 dakika", OptionC = "2-3 saat", OptionD = "Hemen durulayabilir", CorrectOption = "A", Explanation = "Saç boyası genellikle 30-45 dakika etkisini gösterir.", Difficulty = 2, Category = "Saç Boyama" },
                new QuizQuestion { Id = 5, Question = "Jilet ile tıraşta en sık kullanılan açı nedir?", OptionA = "30 derece", OptionB = "90 derece", OptionC = "45 derece", OptionD = "15 derece", CorrectOption = "A", Explanation = "Jilet ile tıraşta genellikle 30 derecelik açı kullanılır.", Difficulty = 2, Category = "Sakal" }
            );
        }
    }
}
