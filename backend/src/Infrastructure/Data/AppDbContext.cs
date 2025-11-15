using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesOrderLine> SalesOrderLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------------
            // SalesOrder → SalesOrderLine (1:M)
            // -------------------------------
            modelBuilder.Entity<SalesOrder>()
                .HasMany(o => o.Lines)
                .WithOne(l => l.SalesOrder)
                .HasForeignKey(l => l.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // -------------------------------
            // SalesOrderLine → Item (M:1)
            // -------------------------------
            modelBuilder.Entity<SalesOrderLine>()
                .HasOne(l => l.Item)
                .WithMany(i => i.OrderLines)
                .HasForeignKey(l => l.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------------
            // Decimal Precision Fixes
            // -------------------------------

            // Items
            modelBuilder.Entity<Item>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

            // SalesOrder Totals
            modelBuilder.Entity<SalesOrder>()
                .Property(o => o.TotalExcl).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrder>()
                .Property(o => o.TotalTax).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrder>()
                .Property(o => o.TotalIncl).HasPrecision(18, 2);

            // SalesOrderLine numeric fields
            modelBuilder.Entity<SalesOrderLine>()
                .Property(l => l.Quantity).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderLine>()
                .Property(l => l.Price).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderLine>()
                .Property(l => l.ExclAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderLine>()
                .Property(l => l.TaxAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderLine>()
                .Property(l => l.InclAmount).HasPrecision(18, 2);
            modelBuilder.Entity<SalesOrderLine>()
                .Property(l => l.TaxRate).HasPrecision(5, 2);
        }
    }
}
