using Microsoft.EntityFrameworkCore;
using Backend.Infrastructure.Entities;

namespace Backend.Infrastructure.Data
{
    public class BackendDbContext : DbContext
    {
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("user");
                entity.HasKey(e => e.Id);
            });
        }
    }
}