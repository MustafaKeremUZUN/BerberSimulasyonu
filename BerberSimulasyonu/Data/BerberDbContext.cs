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

                // Foreign key relationships
                entity.HasOne(a => a.Customer)
                      .WithMany(c => c.Appointments)
                      .HasForeignKey(a => a.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Service)
                      .WithMany(s => s.Appointments)
                      .HasForeignKey(a => a.ServiceId)
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
        }
    }
}
