using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Servicos;

namespace ControleMateriais.Tests.Domain;

public class CalculadoraEstoqueTests
{
    [Fact]
    public void DeveSomarEntradasAoSaldo()
    {
        var unidadeId = Guid.NewGuid();
        var materialId = Guid.NewGuid();

        var movimentacoes = new[]
        {
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Entrada,
                10
            ),
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Entrada,
                5
            )
        };

        var saldo = CalculadoraEstoque.CalcularSaldo(movimentacoes);

        Assert.Equal(15, saldo);
    }

    [Fact]
    public void DeveSubtrairSaidasDoSaldo()
    {
        var unidadeId = Guid.NewGuid();
        var materialId = Guid.NewGuid();

        var movimentacoes = new[]
        {
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Entrada,
                10
            ),
            new MovimentacaoEstoque(
                unidadeId,
                materialId,
                TipoMovimentacao.Saida,
                3
            )
        };

        var saldo = CalculadoraEstoque.CalcularSaldo(movimentacoes);

        Assert.Equal(7, saldo);
    }

    [Fact]
    public void DeveRetornarZeroQuandoNaoExistiremMovimentacoes()
    {
        var movimentacoes =
            Array.Empty<MovimentacaoEstoque>();

        var saldo = CalculadoraEstoque.CalcularSaldo(movimentacoes);

        Assert.Equal(0, saldo);
    }
}
