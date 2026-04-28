using MediBook.RecordService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.RecordService.Data
{
    public class RecordDbContext : DbContext
    {
        public RecordDbContext(DbContextOptions<RecordDbContext> options) : base(options)
        {
        }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(r => r.RecordId);
                entity.HasIndex(r => r.AppointmentId).IsUnique();
                entity.HasIndex(r => r.PatientId);
                entity.HasIndex(r => r.ProviderId);
                entity.HasIndex(r => r.FollowUpDate);

                entity.Property(r => r.RecordId).IsRequired();
                entity.Property(r => r.AppointmentId).IsRequired();
                entity.Property(r => r.PatientId).IsRequired();
                entity.Property(r => r.ProviderId).IsRequired();
                entity.Property(r => r.Diagnosis).IsRequired();
                entity.Property(r => r.Prescription).IsRequired();
                entity.Property(r => r.Notes).IsRequired();
                entity.Property(r => r.AttachmentUrl).HasMaxLength(500);
                entity.Property(r => r.CreatedAt).IsRequired();
                entity.Property(r => r.UpdatedAt).IsRequired();
            });
        }
    }
}
