using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IRepositorioMaterial
{
    Task<Material?> ObterPorIdAsync(Guid id);

    Task<IReadOnlyCollection<Material>> ListarAsync();

    Task AdicionarAsync(Material material);
}
