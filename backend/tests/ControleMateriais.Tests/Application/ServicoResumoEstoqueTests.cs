using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class ServicoResumoEstoqueTests
{
    [Fact]
    public async Task DeveGerarResumoComEntradasSaidasESaldo()
    {
        var repositorio =
            new RepositorioMovimentacaoEstoqueFake();

        var unidadeId = Guid.NewGuid();
        var materialId = Guid.NewGuid();

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Entrada,
                10
            )
        );

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Saida,
                3
            )
        );

        var servico =
            new ServicoResumoEstoque(repositorio);

        var resultado =
            await servico.GerarAsync(
                unidadeId,
                materialId
            );

        Assert.Equal(unidadeId, resultado.UnidadeId);
        Assert.Equal(materialId, resultado.MaterialId);

        Assert.Equal(
            2,
            resultado.TotalMovimentacoes
        );

        Assert.Equal(
            10m,
            resultado.TotalEntradas
        );

        Assert.Equal(
            3m,
            resultado.TotalSaidas
        );

        Assert.Equal(
            7m,
            resultado.Saldo
        );
    }
}
