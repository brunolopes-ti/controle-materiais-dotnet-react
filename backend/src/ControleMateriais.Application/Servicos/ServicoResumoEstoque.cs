using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Application.Servicos;

public class ServicoResumoEstoque
{
    private readonly IRepositorioMovimentacaoEstoque
        _repositorioMovimentacao;

    public ServicoResumoEstoque(
        IRepositorioMovimentacaoEstoque repositorioMovimentacao)
    {
        _repositorioMovimentacao = repositorioMovimentacao;
    }

    public async Task<ResumoEstoqueResponse> GerarAsync(
        Guid? unidadeId = null,
        Guid? materialId = null)
    {
        var movimentacoes =
            await _repositorioMovimentacao.ListarAsync(
                unidadeId,
                materialId
            );

        var totalEntradas = movimentacoes
            .Where(movimentacao =>
                movimentacao.Tipo == TipoMovimentacao.Entrada
            )
            .Sum(movimentacao => movimentacao.Quantidade);

        var totalSaidas = movimentacoes
            .Where(movimentacao =>
                movimentacao.Tipo == TipoMovimentacao.Saida
            )
            .Sum(movimentacao => movimentacao.Quantidade);

        return new ResumoEstoqueResponse
        {
            UnidadeId = unidadeId,
            MaterialId = materialId,
            TotalMovimentacoes = movimentacoes.Count,
            TotalEntradas = totalEntradas,
            TotalSaidas = totalSaidas,
            Saldo = totalEntradas - totalSaidas
        };
    }
}
