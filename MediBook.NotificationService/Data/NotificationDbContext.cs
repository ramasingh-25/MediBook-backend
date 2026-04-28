using MediBook.NotificationService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.NotificationService.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.NotificationId);
                entity.HasIndex(n => n.RecipientId);
                entity.HasIndex(n => n.Type);
                entity.HasIndex(n => n.RelatedId);
                entity.HasIndex(n => n.IsRead);

                entity.Property(n => n.NotificationId).IsRequired();
                entity.Property(n => n.RecipientId).IsRequired();
                entity.Property(n => n.Type).IsRequired().HasMaxLength(50);
                entity.Property(n => n.Title).IsRequired().HasMaxLength(200);
                entity.Property(n => n.Message).IsRequired().HasMaxLength(1000);
                entity.Property(n => n.Channel).IsRequired().HasMaxLength(20);
                entity.Property(n => n.RelatedId).HasMaxLength(100);
                entity.Property(n => n.RelatedType).HasMaxLength(50);
                entity.Property(n => n.IsRead).HasDefaultValue(false);
                entity.Property(n => n.SentAt).IsRequired();
            });
        }
    }
}
