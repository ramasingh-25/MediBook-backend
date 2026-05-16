using MediBook.ReviewService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.ReviewService.Data
{
    public class ReviewDbContext : DbContext
    {
        public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global UTC converter for all DateTime properties to satisfy Npgsql/PostgreSQL
            var dateTimeConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                }
            }

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.ReviewId);
                entity.HasIndex(r => r.AppointmentId).IsUnique(); // One review per appointment
                entity.HasIndex(r => r.PatientId);
                entity.HasIndex(r => r.ProviderId);
                entity.HasIndex(r => r.Rating);

                entity.Property(r => r.ReviewId).IsRequired();
                entity.Property(r => r.AppointmentId).IsRequired();
                entity.Property(r => r.PatientId).IsRequired();
                entity.Property(r => r.ProviderId).IsRequired();
                entity.Property(r => r.Rating).IsRequired();
                entity.Property(r => r.Comment).HasMaxLength(1000);
                entity.Property(r => r.ReviewDate).IsRequired();
                entity.Property(r => r.IsVerified).HasDefaultValue(true);
                entity.Property(r => r.IsAnonymous).HasDefaultValue(false);
            });
        }
    }
}
