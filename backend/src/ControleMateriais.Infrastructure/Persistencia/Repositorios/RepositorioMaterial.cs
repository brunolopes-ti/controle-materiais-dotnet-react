using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Infrastructure.Persistencia.Repositorios;

public class RepositorioMaterial : IRepositorioMaterial
{
    private readonly ControleMateriaisDbContext _context;

    public RepositorioMaterial(ControleMateriaisDbContext context)
    {
        _context = context;
    }

    public async Task<Material?> ObterPorIdAsync(Guid id)
    {
        return await _context.Materiais.FindAsync(id);
    }

    public async Task AdicionarAsync(Material material)
    {
        await _context.Materiais.AddAsync(material);

        await _context.SaveChangesAsync();
    }
}
