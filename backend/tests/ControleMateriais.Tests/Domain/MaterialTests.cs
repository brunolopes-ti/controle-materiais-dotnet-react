using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Domain;

public class MaterialTests
{
    [Fact]
    public void DeveCriarMaterialComDadosValidos()
    {
        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        Assert.NotEqual(Guid.Empty, material.Id);
        Assert.Equal("Álcool 70%", material.Nome);
        Assert.Equal(UnidadeMedida.Litro, material.UnidadeMedida);
        Assert.True(material.Ativo);
    }

    [Fact]
    public void DeveRemoverEspacosDoNome()
    {
        var material = new Material(
            "  Detergente  ",
            UnidadeMedida.Litro
        );

        Assert.Equal("Detergente", material.Nome);
    }

    [Fact]
    public void NaoDeveCriarMaterialSemNome()
    {
        var excecao = Assert.Throws<ArgumentException>(
            () => new Material(
                "",
                UnidadeMedida.Unidade
            )
        );

        Assert.Equal(
            "O nome do material é obrigatório. (Parameter 'nome')",
            excecao.Message
        );
    }

    [Fact]
    public void DeveDesativarMaterial()
    {
        var material = new Material(
            "Papel Higiênico",
            UnidadeMedida.Rolo
        );

        material.Desativar();

        Assert.False(material.Ativo);
    }
}
