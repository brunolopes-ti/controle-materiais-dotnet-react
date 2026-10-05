using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IRepositorioMaterial
{
    Task<Material?> ObterPorIdAsync(Guid id);

    Task AdicionarAsync(Material material);
}
