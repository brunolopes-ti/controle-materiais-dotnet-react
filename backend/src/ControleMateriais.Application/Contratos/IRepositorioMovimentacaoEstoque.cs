using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IRepositorioMovimentacaoEstoque
{
    Task AdicionarAsync(MovimentacaoEstoque movimentacao);

    Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarPorUnidadeEMaterialAsync(
            Guid unidadeId,
            Guid materialId
        );

    Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarAsync(
            Guid? unidadeId = null,
            Guid? materialId = null
        );
}
