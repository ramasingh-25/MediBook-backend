using MediBook.AuthService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediBook.AuthService.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Phone).IsUnique();

                entity.Property(u => u.UserId).IsRequired();
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Provider).HasMaxLength(20).HasDefaultValue("Local");
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.ProfilePicUrl).HasMaxLength(500);
            });
        }
    }
}