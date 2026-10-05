using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ControleMateriais.Infrastructure.Persistencia;

public class ControleMateriaisDbContext : DbContext
{
    public ControleMateriaisDbContext(
        DbContextOptions<ControleMateriaisDbContext> options)
        : base(options)
    {
    }

    public DbSet<Unidade> Unidades => Set<Unidade>();

    public DbSet<Material> Materiais => Set<Material>();

    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque
        => Set<MovimentacaoEstoque>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ControleMateriaisDbContext).Assembly
        );
    }
}
