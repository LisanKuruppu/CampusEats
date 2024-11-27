using Microsoft.EntityFrameworkCore;
using Core.Domain.Entities;

namespace Infrastructure.Persistence
{
    public class CampusEatsDbContext : DbContext
    {
        public CampusEatsDbContext(DbContextOptions<CampusEatsDbContext> options) 
            : base(options) { }

        // Define DbSets for your entities
        public DbSet<Product> Products { get; set; }
        // public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations here (e.g., Fluent API rules)
        }
    }
}
