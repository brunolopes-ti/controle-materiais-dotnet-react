using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class RepositorioMovimentacaoEstoqueFake
    : IRepositorioMovimentacaoEstoque
{
    private readonly List<MovimentacaoEstoque> _movimentacoes = new();

    public Task AdicionarAsync(
        MovimentacaoEstoque movimentacao)
    {
        _movimentacoes.Add(movimentacao);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarPorUnidadeEMaterialAsync(
            Guid unidadeId,
            Guid materialId)
    {
        IReadOnlyCollection<MovimentacaoEstoque> resultado =
            _movimentacoes
                .Where(movimentacao =>
                    movimentacao.UnidadeId == unidadeId &&
                    movimentacao.MaterialId == materialId
                )
                .ToList();

        return Task.FromResult(resultado);
    }
}
