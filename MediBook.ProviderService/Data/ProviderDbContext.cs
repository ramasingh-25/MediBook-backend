using MediBook.ProviderService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.ProviderService.Data
{
    public class ProviderDbContext : DbContext
    {
        public ProviderDbContext(DbContextOptions<ProviderDbContext> options) : base(options)
        {
        }

        public DbSet<Provider> Providers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.HasKey(p => p.ProviderId);
                entity.HasIndex(p => p.UserId).IsUnique();
                entity.HasIndex(p => p.Specialization);
                entity.HasIndex(p => p.IsVerified);
                entity.HasIndex(p => p.IsAvailable);

                entity.Property(p => p.ProviderId).IsRequired();
                entity.Property(p => p.UserId).IsRequired();
                entity.Property(p => p.Specialization).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Qualification).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Bio).HasMaxLength(1000);
                entity.Property(p => p.ClinicName).IsRequired().HasMaxLength(200);
                entity.Property(p => p.ClinicAddress).IsRequired().HasMaxLength(500);
                entity.Property(p => p.AvgRating).HasDefaultValue(0.0);
                entity.Property(p => p.IsVerified).HasDefaultValue(false);
                entity.Property(p => p.IsAvailable).HasDefaultValue(true);
            });
        }
    }
}
