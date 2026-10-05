using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Domain;

public class UnidadeTests
{
    [Fact]
    public void DeveCriarUnidadeComNomeValido()
    {
        var unidade = new Unidade("Prédio A");

        Assert.NotEqual(Guid.Empty, unidade.Id);
        Assert.Equal("Prédio A", unidade.Nome);
        Assert.True(unidade.Ativa);
    }

    [Fact]
    public void NaoDeveCriarUnidadeSemNome()
    {
        var excecao = Assert.Throws<ArgumentException>(
            () => new Unidade("")
        );

        Assert.Equal(
            "O nome da unidade é obrigatório. (Parameter 'nome')",
            excecao.Message
        );
    }

    [Fact]
    public void DeveDesativarUnidade()
    {
        var unidade = new Unidade("Prédio A");

        unidade.Desativar();

        Assert.False(unidade.Ativa);
    }
}
