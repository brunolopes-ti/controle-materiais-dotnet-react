using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class ServicoMaterialTests
{
    [Fact]
    public async Task DeveCadastrarMaterial()
    {
        var repositorio = new RepositorioMaterialFake();

        var servico = new ServicoMaterial(repositorio);

        var request = new CriarMaterialRequest
        {
            Nome = "Álcool 70%",
            UnidadeMedida = UnidadeMedida.Litro
        };

        var resultado = await servico.CriarAsync(request);

        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal("Álcool 70%", resultado.Nome);
        Assert.Equal(
            UnidadeMedida.Litro,
            resultado.UnidadeMedida
        );
        Assert.True(resultado.Ativo);
    }

    [Fact]
    public async Task NaoDeveCadastrarMaterialSemNome()
    {
        var repositorio = new RepositorioMaterialFake();

        var servico = new ServicoMaterial(repositorio);

        var request = new CriarMaterialRequest
        {
            Nome = "",
            UnidadeMedida = UnidadeMedida.Unidade
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => servico.CriarAsync(request)
        );
    }
}
