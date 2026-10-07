using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;

namespace ControleMateriais.Application.Servicos;

public class ServicoHistoricoMovimentacao
{
    private readonly IRepositorioMovimentacaoEstoque
        _repositorioMovimentacao;

    public ServicoHistoricoMovimentacao(
        IRepositorioMovimentacaoEstoque repositorioMovimentacao)
    {
        _repositorioMovimentacao =
            repositorioMovimentacao;
    }

    public async Task<IReadOnlyCollection<HistoricoMovimentacaoResponse>>
        ListarAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var movimentacoes =
            await _repositorioMovimentacao.ListarAsync(
                unidadeId,
                materialId
            );

        return movimentacoes
            .Select(Mapear)
            .ToList();
    }

    public async Task<HistoricoPaginadoResponse>
        ListarPaginadoAsync(
            int pagina,
            int tamanhoPagina,
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        if (pagina < 1)
        {
            throw new ArgumentException(
                "A página deve ser maior ou igual a 1.",
                nameof(pagina)
            );
        }

        if (
            tamanhoPagina < 1 ||
            tamanhoPagina > 100
        )
        {
            throw new ArgumentException(
                "O tamanho da página deve estar entre 1 e 100.",
                nameof(tamanhoPagina)
            );
        }

        var resultado =
            await _repositorioMovimentacao
                .ListarPaginadoAsync(
                    pagina,
                    tamanhoPagina,
                    unidadeId,
                    materialId
                );

        var totalPaginas =
            resultado.TotalItens == 0
                ? 0
                : (int)Math.Ceiling(
                    resultado.TotalItens /
                    (double)tamanhoPagina
                );

        return new HistoricoPaginadoResponse
        {
            Itens = resultado.Itens
                .Select(Mapear)
                .ToList(),

            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = resultado.TotalItens,
            TotalPaginas = totalPaginas
        };
    }

    private static HistoricoMovimentacaoResponse Mapear(
        ControleMateriais.Domain.Entidades.MovimentacaoEstoque
            movimentacao)
    {
        return new HistoricoMovimentacaoResponse
        {
            Id = movimentacao.Id,
            UnidadeId = movimentacao.UnidadeId,
            MaterialId = movimentacao.MaterialId,
            Tipo = movimentacao.Tipo,
            Quantidade = movimentacao.Quantidade,
            DataMovimentacao =
                movimentacao.DataMovimentacao
        };
    }
}
