using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IRepositorioUnidade
{
    Task<Unidade?> ObterPorIdAsync(Guid id);

    Task AdicionarAsync(Unidade unidade);
}
