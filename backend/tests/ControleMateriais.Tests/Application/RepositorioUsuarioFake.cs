using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class RepositorioUsuarioFake : IRepositorioUsuario
{
    private readonly List<Usuario> _usuarios = new();

    public Task AdicionarAsync(Usuario usuario)
    {
        _usuarios.Add(usuario);

        return Task.CompletedTask;
    }

    public Task<Usuario?> ObterPorEmailAsync(string email)
    {
        var emailNormalizado =
            email.Trim().ToLowerInvariant();

        var usuario =
            _usuarios.FirstOrDefault(
                usuario =>
                    usuario.Email == emailNormalizado
            );

        return Task.FromResult(usuario);
    }

    public Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        var usuario =
            _usuarios.FirstOrDefault(
                usuario => usuario.Id == id
            );

        return Task.FromResult(usuario);
    }
}
