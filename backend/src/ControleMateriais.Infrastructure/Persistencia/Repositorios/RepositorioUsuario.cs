using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ControleMateriais.Infrastructure.Persistencia.Repositorios;

public class RepositorioUsuario : IRepositorioUsuario
{
    private readonly ControleMateriaisDbContext _context;

    public RepositorioUsuario(
        ControleMateriaisDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);

        await _context.SaveChangesAsync();
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        var emailNormalizado =
            email.Trim().ToLowerInvariant();

        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                usuario =>
                    usuario.Email == emailNormalizado
            );
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(
                usuario => usuario.Id == id
            );
    }
}
