using MediBook.AvailabilityService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.AvailabilityService.Data
{
    public class SlotDbContext : DbContext
    {
        public SlotDbContext(DbContextOptions<SlotDbContext> options) : base(options)
        {
        }

        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }

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

            modelBuilder.Entity<AvailabilitySlot>(entity =>
            {
                entity.HasKey(s => s.SlotId);
                entity.HasIndex(s => s.ProviderId);
                entity.HasIndex(s => new { s.ProviderId, s.Date });
                entity.HasIndex(s => s.IsBooked);
                entity.HasIndex(s => s.IsBlocked);

                entity.Property(s => s.SlotId).IsRequired();
                entity.Property(s => s.ProviderId).IsRequired();
                entity.Property(s => s.Date).IsRequired();
                entity.Property(s => s.StartTime).IsRequired();
                entity.Property(s => s.EndTime).IsRequired();
                entity.Property(s => s.DurationMinutes).IsRequired();
                entity.Property(s => s.IsBooked).HasDefaultValue(false);
                entity.Property(s => s.IsBlocked).HasDefaultValue(false);
                entity.Property(s => s.Recurrence).HasMaxLength(50);
            });
        }
    }
}
