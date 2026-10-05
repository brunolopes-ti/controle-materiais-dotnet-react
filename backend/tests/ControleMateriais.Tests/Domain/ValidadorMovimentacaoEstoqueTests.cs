using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Servicos;

namespace ControleMateriais.Tests.Domain;

public class ValidadorMovimentacaoEstoqueTests
{
    [Fact]
    public void DevePermitirEntradaComUnidadeEMaterialAtivos()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        var excecao = Record.Exception(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                TipoMovimentacao.Entrada,
                10,
                0
            )
        );

        Assert.Null(excecao);
    }

    [Fact]
    public void DevePermitirSaidaComEstoqueSuficiente()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        var excecao = Record.Exception(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                TipoMovimentacao.Saida,
                4,
                10
            )
        );

        Assert.Null(excecao);
    }

    [Fact]
    public void NaoDeveMovimentarEstoqueDeUnidadeInativa()
    {
        var unidade = new Unidade("Prédio A");
        unidade.Desativar();

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        var excecao = Assert.Throws<InvalidOperationException>(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                TipoMovimentacao.Entrada,
                10,
                0
            )
        );

        Assert.Equal(
            "Não é possível movimentar estoque de uma unidade inativa.",
            excecao.Message
        );
    }

    [Fact]
    public void NaoDeveMovimentarMaterialInativo()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        material.Desativar();

        var excecao = Assert.Throws<InvalidOperationException>(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                TipoMovimentacao.Entrada,
                10,
                0
            )
        );

        Assert.Equal(
            "Não é possível movimentar um material inativo.",
            excecao.Message
        );
    }

    [Fact]
    public void NaoDevePermitirSaidaSemEstoqueSuficiente()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        Assert.Throws<InvalidOperationException>(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                TipoMovimentacao.Saida,
                10,
                5
            )
        );
    }

    [Fact]
    public void NaoDevePermitirQuantidadeNegativa()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        Assert.Throws<ArgumentException>(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                TipoMovimentacao.Entrada,
                -1,
                0
            )
        );
    }

    [Fact]
    public void NaoDevePermitirTipoInvalido()
    {
        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        Assert.Throws<ArgumentOutOfRangeException>(
            () => ValidadorMovimentacaoEstoque.Validar(
                unidade,
                material,
                (TipoMovimentacao)99,
                10,
                0
            )
        );
    }
}
