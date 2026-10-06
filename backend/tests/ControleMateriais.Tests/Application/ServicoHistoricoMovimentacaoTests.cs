using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class ServicoHistoricoMovimentacaoTests
{
    [Fact]
    public async Task DeveFiltrarHistoricoPorUnidadeEMaterial()
    {
        var repositorio =
            new RepositorioMovimentacaoEstoqueFake();

        var unidadeA = Guid.NewGuid();
        var unidadeB = Guid.NewGuid();

        var materialA = Guid.NewGuid();
        var materialB = Guid.NewGuid();

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeA,
                materialA,
                TipoMovimentacao.Entrada,
                10
            )
        );

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeA,
                materialB,
                TipoMovimentacao.Entrada,
                5
            )
        );

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeB,
                materialA,
                TipoMovimentacao.Entrada,
                8
            )
        );

        var servico =
            new ServicoHistoricoMovimentacao(repositorio);

        var resultado =
            await servico.ListarAsync(
                unidadeA,
                materialA
            );

        var movimentacao = Assert.Single(resultado);

        Assert.Equal(
            unidadeA,
            movimentacao.UnidadeId
        );

        Assert.Equal(
            materialA,
            movimentacao.MaterialId
        );

        Assert.Equal(
            10m,
            movimentacao.Quantidade
        );
    }

    [Fact]
    public async Task DeveListarMovimentacoesMaisRecentesPrimeiro()
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

        await Task.Delay(10);

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Saida,
                3
            )
        );

        var servico =
            new ServicoHistoricoMovimentacao(repositorio);

        var resultado =
            await servico.ListarAsync();

        var movimentacoes = resultado.ToList();

        Assert.Equal(
            2,
            movimentacoes.Count
        );

        Assert.Equal(
            TipoMovimentacao.Saida,
            movimentacoes[0].Tipo
        );

        Assert.Equal(
            TipoMovimentacao.Entrada,
            movimentacoes[1].Tipo
        );
    }
}
