using Microsoft.EntityFrameworkCore;
using StockService.Models;

namespace StockService.Data;

public class ProdutoContext : DbContext
{
    public ProdutoContext(DbContextOptions<ProdutoContext> opts) : base(opts)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Categoria>()
            .HasMany(categoria => categoria.Products)
            .WithOne(produto => produto.Category)
            .HasForeignKey(produto => produto.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Produto> Products { get; set; }
    public DbSet<Categoria> Categories { get; set; }
}