using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Excecoes;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException erro)
            when (erro.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            // O serviço já verifica o e-mail antes de inserir, mas
            // dois cadastros simultâneos podem passar por essa
            // verificação. O índice único do banco é a garantia
            // final; aqui traduzimos a violação para um 409.
            _context.Entry(usuario).State = EntityState.Detached;

            throw new ConflitoException(
                "Já existe um usuário cadastrado com este e-mail."
            );
        }
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

    public async Task<bool> ExisteAlgumAsync()
    {
        return await _context.Usuarios.AnyAsync();
    }
}
