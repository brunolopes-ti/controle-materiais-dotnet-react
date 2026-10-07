using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class ServicoHistoricoPaginacaoTests
{
    [Fact]
    public async Task DeveRetornarPrimeiraPaginaDoHistorico()
    {
        var repositorio =
            new RepositorioMovimentacaoEstoqueFake();

        var unidadeId =
            Guid.NewGuid();

        var materialId =
            Guid.NewGuid();

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
                2
            )
        );

        await Task.Delay(10);

        await repositorio.AdicionarAsync(
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Entrada,
                5
            )
        );

        var servico =
            new ServicoHistoricoMovimentacao(
                repositorio
            );

        var resultado =
            await servico.ListarPaginadoAsync(
                pagina: 1,
                tamanhoPagina: 2,
                unidadeId: unidadeId,
                materialId: materialId
            );

        Assert.Equal(
            1,
            resultado.Pagina
        );

        Assert.Equal(
            2,
            resultado.TamanhoPagina
        );

        Assert.Equal(
            3,
            resultado.TotalItens
        );

        Assert.Equal(
            2,
            resultado.TotalPaginas
        );

        Assert.Equal(
            2,
            resultado.Itens.Count
        );

        Assert.Equal(
            5m,
            resultado.Itens.First().Quantidade
        );
    }

    [Fact]
    public async Task NaoDeveAceitarPaginaMenorQueUm()
    {
        var repositorio =
            new RepositorioMovimentacaoEstoqueFake();

        var servico =
            new ServicoHistoricoMovimentacao(
                repositorio
            );

        var excecao =
            await Assert.ThrowsAsync<ArgumentException>(
                () => servico.ListarPaginadoAsync(
                    pagina: 0,
                    tamanhoPagina: 20
                )
            );

        Assert.Contains(
            "A página deve ser maior ou igual a 1.",
            excecao.Message
        );
    }

    [Fact]
    public async Task NaoDeveAceitarTamanhoPaginaMaiorQueCem()
    {
        var repositorio =
            new RepositorioMovimentacaoEstoqueFake();

        var servico =
            new ServicoHistoricoMovimentacao(
                repositorio
            );

        var excecao =
            await Assert.ThrowsAsync<ArgumentException>(
                () => servico.ListarPaginadoAsync(
                    pagina: 1,
                    tamanhoPagina: 101
                )
            );

        Assert.Contains(
            "O tamanho da página deve estar entre 1 e 100.",
            excecao.Message
        );
    }
}
