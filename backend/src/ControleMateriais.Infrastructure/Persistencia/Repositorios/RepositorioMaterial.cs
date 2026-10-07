using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ControleMateriais.Infrastructure.Persistencia.Repositorios;

public class RepositorioMaterial : IRepositorioMaterial
{
    private readonly ControleMateriaisDbContext _context;

    public RepositorioMaterial(
        ControleMateriaisDbContext context)
    {
        _context = context;
    }

    public async Task<Material?> ObterPorIdAsync(
        Guid id)
    {
        return await _context.Materiais.FindAsync(id);
    }

    public async Task<IReadOnlyCollection<Material>>
        ListarAsync()
    {
        return await _context.Materiais
            .AsNoTracking()
            .OrderBy(material => material.Nome)
            .ToListAsync();
    }

    public async Task AdicionarAsync(
        Material material)
    {
        await _context.Materiais.AddAsync(material);

        await _context.SaveChangesAsync();
    }
}
