using MediBook.PaymentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.PaymentService.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.PaymentId);
                entity.HasIndex(p => p.AppointmentId);
                entity.HasIndex(p => p.PatientId);
                entity.HasIndex(p => p.Status);
                entity.HasIndex(p => p.TransactionId);

                entity.Property(p => p.PaymentId).IsRequired();
                entity.Property(p => p.AppointmentId).IsRequired();
                entity.Property(p => p.PatientId).IsRequired();
                entity.Property(p => p.Amount).IsRequired();
                entity.Property(p => p.Status).IsRequired().HasMaxLength(20);
                entity.Property(p => p.Mode).IsRequired().HasMaxLength(20);
                entity.Property(p => p.TransactionId).HasMaxLength(100);
                entity.Property(p => p.Currency).IsRequired().HasMaxLength(10);
                entity.Property(p => p.Notes).HasMaxLength(500);
            });
        }
    }
}
