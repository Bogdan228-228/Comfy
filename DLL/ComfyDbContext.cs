using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DOMAIN;
using Microsoft.EntityFrameworkCore;

namespace DLL
{
    public class ComfyDbContext : IdentityDbContext<User>
    {
        public ComfyDbContext(DbContextOptions<ComfyDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
