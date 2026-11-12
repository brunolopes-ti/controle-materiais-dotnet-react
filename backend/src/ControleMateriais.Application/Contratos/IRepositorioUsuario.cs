using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IRepositorioUsuario
{
    /// <summary>
    /// Persiste o usuário. Lança ConflitoException se o e-mail
    /// já existir (inclusive quando dois cadastros chegam juntos).
    /// </summary>
    Task AdicionarAsync(Usuario usuario);

    Task<Usuario?> ObterPorEmailAsync(string email);

    Task<Usuario?> ObterPorIdAsync(Guid id);

    Task<bool> ExisteAlgumAsync();
}
