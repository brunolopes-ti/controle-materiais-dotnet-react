using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IRepositorioUsuario
{
    Task AdicionarAsync(Usuario usuario);

    Task<Usuario?> ObterPorEmailAsync(string email);

    Task<Usuario?> ObterPorIdAsync(Guid id);
}
