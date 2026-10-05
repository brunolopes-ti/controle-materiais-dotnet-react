using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Domain;

public class MovimentacaoEstoqueTests
{
    [Fact]
    public void DeveCriarMovimentacaoDeEntrada()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        var movimentacao = new MovimentacaoEstoque(
            unidade.Id,
            material.Id,
            TipoMovimentacao.Entrada,
            10
        );

        Assert.NotEqual(Guid.Empty, movimentacao.Id);
        Assert.Equal(unidade.Id, movimentacao.UnidadeId);
        Assert.Equal(material.Id, movimentacao.MaterialId);
        Assert.Equal(TipoMovimentacao.Entrada, movimentacao.Tipo);
        Assert.Equal(10, movimentacao.Quantidade);
    }

    [Fact]
    public void DeveCriarMovimentacaoDeSaida()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Detergente",
            UnidadeMedida.Litro
        );

        var movimentacao = new MovimentacaoEstoque(
            unidade.Id,
            material.Id,
            TipoMovimentacao.Saida,
            2
        );

        Assert.Equal(TipoMovimentacao.Saida, movimentacao.Tipo);
        Assert.Equal(2, movimentacao.Quantidade);
    }

    [Fact]
    public void NaoDeveCriarMovimentacaoSemUnidade()
    {
        var materialId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(
            () => new MovimentacaoEstoque(
                Guid.Empty,
                materialId,
                TipoMovimentacao.Entrada,
                10
            )
        );
    }

    [Fact]
    public void NaoDeveCriarMovimentacaoSemMaterial()
    {
        var unidadeId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(
            () => new MovimentacaoEstoque(
                unidadeId,
                Guid.Empty,
                TipoMovimentacao.Entrada,
                10
            )
        );
    }

    [Fact]
    public void NaoDeveCriarMovimentacaoComQuantidadeZero()
    {
        Assert.Throws<ArgumentException>(
            () => new MovimentacaoEstoque(
                Guid.NewGuid(),
                Guid.NewGuid(),
                TipoMovimentacao.Entrada,
                0
            )
        );
    }

    [Fact]
    public void NaoDeveCriarMovimentacaoComTipoInvalido()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new MovimentacaoEstoque(
                Guid.NewGuid(),
                Guid.NewGuid(),
                (TipoMovimentacao)99,
                10
            )
        );
    }
}
    
