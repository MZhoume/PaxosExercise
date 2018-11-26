using Challenge1.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Challenge1.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options)
            : base(options)
        {
        }

        public DbSet<HashDto> Hashes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HashDto>(h => h.HasKey(e => e.Hash));
        }
    }
}
