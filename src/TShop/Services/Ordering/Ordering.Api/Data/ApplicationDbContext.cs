using Microsoft.EntityFrameworkCore;
using Ordering.Api.Entity;

namespace Ordering.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(oi => oi.OriginalPrice)
                .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Order>()
                .Property(oi => oi.FinalPrice)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.OriginalPrice)
                .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.FinalPrice)
                .HasColumnType("decimal(18, 2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}