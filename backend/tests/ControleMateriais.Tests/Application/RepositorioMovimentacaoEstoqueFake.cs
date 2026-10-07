using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class RepositorioMovimentacaoEstoqueFake
    : IRepositorioMovimentacaoEstoque
{
    private readonly List<MovimentacaoEstoque>
        _movimentacoes = new();

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
                    movimentacao.UnidadeId ==
                        unidadeId &&
                    movimentacao.MaterialId ==
                        materialId
                )
                .ToList();

        return Task.FromResult(resultado);
    }

    public Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            CriarConsulta(
                unidadeId,
                materialId
            );

        IReadOnlyCollection<MovimentacaoEstoque> resultado =
            consulta
                .OrderByDescending(
                    movimentacao =>
                        movimentacao.DataMovimentacao
                )
                .ToList();

        return Task.FromResult(resultado);
    }

    public Task<(
        IReadOnlyCollection<MovimentacaoEstoque> Itens,
        int TotalItens
    )>
        ListarPaginadoAsync(
            int pagina,
            int tamanhoPagina,
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            CriarConsulta(
                unidadeId,
                materialId
            );

        var totalItens =
            consulta.Count();

        IReadOnlyCollection<MovimentacaoEstoque> itens =
            consulta
                .OrderByDescending(
                    movimentacao =>
                        movimentacao.DataMovimentacao
                )
                .Skip(
                    (pagina - 1) * tamanhoPagina
                )
                .Take(tamanhoPagina)
                .ToList();

        return Task.FromResult(
            (
                itens,
                totalItens
            )
        );
    }

    public Task<(
        int TotalMovimentacoes,
        decimal TotalEntradas,
        decimal TotalSaidas
    )>
        ObterResumoAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            CriarConsulta(
                unidadeId,
                materialId
            );

        var movimentacoes =
            consulta.ToList();

        var totalEntradas =
            movimentacoes
                .Where(movimentacao =>
                    movimentacao.Tipo ==
                    TipoMovimentacao.Entrada
                )
                .Sum(movimentacao =>
                    movimentacao.Quantidade
                );

        var totalSaidas =
            movimentacoes
                .Where(movimentacao =>
                    movimentacao.Tipo ==
                    TipoMovimentacao.Saida
                )
                .Sum(movimentacao =>
                    movimentacao.Quantidade
                );

        return Task.FromResult(
            (
                movimentacoes.Count,
                totalEntradas,
                totalSaidas
            )
        );
    }

    private IEnumerable<MovimentacaoEstoque> CriarConsulta(
        Guid? unidadeId,
        Guid? materialId)
    {
        IEnumerable<MovimentacaoEstoque> consulta =
            _movimentacoes;

        if (unidadeId.HasValue)
        {
            consulta =
                consulta.Where(
                    movimentacao =>
                        movimentacao.UnidadeId ==
                        unidadeId.Value
                );
        }

        if (materialId.HasValue)
        {
            consulta =
                consulta.Where(
                    movimentacao =>
                        movimentacao.MaterialId ==
                        materialId.Value
                );
        }

        return consulta;
    }
}
