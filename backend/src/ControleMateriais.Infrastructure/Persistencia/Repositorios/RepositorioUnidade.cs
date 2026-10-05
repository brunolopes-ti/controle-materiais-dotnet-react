using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Infrastructure.Persistencia.Repositorios;

public class RepositorioUnidade : IRepositorioUnidade
{
    private readonly ControleMateriaisDbContext _context;

    public RepositorioUnidade(ControleMateriaisDbContext context)
    {
        _context = context;
    }

    public async Task<Unidade?> ObterPorIdAsync(Guid id)
    {
        return await _context.Unidades.FindAsync(id);
    }

    public async Task AdicionarAsync(Unidade unidade)
    {
        await _context.Unidades.AddAsync(unidade);

        await _context.SaveChangesAsync();
    }
}
