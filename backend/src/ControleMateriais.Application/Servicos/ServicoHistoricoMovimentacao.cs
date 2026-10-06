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
        _repositorioMovimentacao = repositorioMovimentacao;
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
            .Select(movimentacao =>
                new HistoricoMovimentacaoResponse
                {
                    Id = movimentacao.Id,
                    UnidadeId = movimentacao.UnidadeId,
                    MaterialId = movimentacao.MaterialId,
                    Tipo = movimentacao.Tipo,
                    Quantidade = movimentacao.Quantidade,
                    DataMovimentacao =
                        movimentacao.DataMovimentacao
                }
            )
            .ToList();
    }
}
