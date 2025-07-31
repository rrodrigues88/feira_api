using Core.Entities; 
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class FeiraDbContext : DbContext
{
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Feirante> Feirantes { get; set; }
    public DbSet<Venda> Vendas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("FeiraDB");
}