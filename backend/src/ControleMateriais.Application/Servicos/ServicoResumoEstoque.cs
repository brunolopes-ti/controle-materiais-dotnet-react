using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;

namespace ControleMateriais.Application.Servicos;

public class ServicoResumoEstoque
{
    private readonly IRepositorioMovimentacaoEstoque
        _repositorioMovimentacao;

    public ServicoResumoEstoque(
        IRepositorioMovimentacaoEstoque repositorioMovimentacao)
    {
        _repositorioMovimentacao =
            repositorioMovimentacao;
    }

    public async Task<ResumoEstoqueResponse> GerarAsync(
        Guid? unidadeId = null,
        Guid? materialId = null)
    {
        var resumo =
            await _repositorioMovimentacao.ObterResumoAsync(
                unidadeId,
                materialId
            );

        return new ResumoEstoqueResponse
        {
            UnidadeId = unidadeId,
            MaterialId = materialId,
            TotalMovimentacoes =
                resumo.TotalMovimentacoes,
            TotalEntradas =
                resumo.TotalEntradas,
            TotalSaidas =
                resumo.TotalSaidas,
            Saldo =
                resumo.TotalEntradas -
                resumo.TotalSaidas
        };
    }
}
