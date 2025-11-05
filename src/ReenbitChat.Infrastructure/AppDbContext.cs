using Microsoft.EntityFrameworkCore;
using ReenbitChat.Domain;

namespace ReenbitChat.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Message>(e =>
            {
                e.HasKey(m => m.Id);
                e.Property(m => m.UserName).HasMaxLength(64).IsRequired();
                e.Property(m => m.Text).HasMaxLength(2000).IsRequired();
                e.Property(m => m.Room).HasMaxLength(64).HasDefaultValue("general");
                e.HasIndex(m => new { m.Room, m.CreatedAtUtc });
            });
        }
    }
}
