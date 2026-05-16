using MediBook.AppointmentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.AppointmentService.Data
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

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

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.AppointmentId);
                entity.HasIndex(a => a.PatientId);
                entity.HasIndex(a => a.ProviderId);
                entity.HasIndex(a => a.SlotId);
                entity.HasIndex(a => a.Status);
                entity.HasIndex(a => new { a.ProviderId, a.AppointmentDate });

                entity.Property(a => a.AppointmentId).IsRequired();
                entity.Property(a => a.PatientId).IsRequired();
                entity.Property(a => a.ProviderId).IsRequired();
                entity.Property(a => a.SlotId).IsRequired();
                entity.Property(a => a.ServiceType).IsRequired().HasMaxLength(100);
                entity.Property(a => a.AppointmentDate).IsRequired();
                entity.Property(a => a.StartTime).IsRequired();
                entity.Property(a => a.EndTime).IsRequired();
                entity.Property(a => a.Status).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Notes).HasMaxLength(1000);
                entity.Property(a => a.ModeOfConsultation).IsRequired().HasMaxLength(50);
            });
        }
    }
}
