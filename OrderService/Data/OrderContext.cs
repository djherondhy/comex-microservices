using Microsoft.EntityFrameworkCore;
using OrderService.Enums;
using OrderService.Models;

namespace OrderService.Data;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> opts) : base(opts)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
        .Property(o => o.Status)
        .HasConversion(
            s => s.ToString(),
            s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s)
        );
    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}