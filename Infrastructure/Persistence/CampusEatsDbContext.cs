using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class CampusEatsDbContext : DbContext, IDbContext
{
    public CampusEatsDbContext(DbContextOptions<CampusEatsDbContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Carts)
            .WithOne()
            .HasForeignKey(c => c.CustomerId);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)
            .WithOne()
            .HasForeignKey(o => o.CustomerId);
    }
}
