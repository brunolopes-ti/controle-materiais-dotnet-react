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

    public Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        IEnumerable<MovimentacaoEstoque> consulta =
            _movimentacoes;

        if (unidadeId.HasValue)
        {
            consulta = consulta.Where(
                movimentacao =>
                    movimentacao.UnidadeId == unidadeId.Value
            );
        }

        if (materialId.HasValue)
        {
            consulta = consulta.Where(
                movimentacao =>
                    movimentacao.MaterialId == materialId.Value
            );
        }

        IReadOnlyCollection<MovimentacaoEstoque> resultado =
            consulta
                .OrderByDescending(
                    movimentacao => movimentacao.DataMovimentacao
                )
                .ToList();

        return Task.FromResult(resultado);
    }
}
