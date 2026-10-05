using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;

namespace ControleMateriais.Tests.Application;

public class ServicoUnidadeTests
{
    [Fact]
    public async Task DeveCadastrarUnidade()
    {
        var repositorio = new RepositorioUnidadeFake();

        var servico = new ServicoUnidade(repositorio);

        var request = new CriarUnidadeRequest
        {
            Nome = "Prédio A"
        };

        var resultado = await servico.CriarAsync(request);

        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal("Prédio A", resultado.Nome);
        Assert.True(resultado.Ativa);
    }

    [Fact]
    public async Task NaoDeveCadastrarUnidadeSemNome()
    {
        var repositorio = new RepositorioUnidadeFake();

        var servico = new ServicoUnidade(repositorio);

        var request = new CriarUnidadeRequest
        {
            Nome = ""
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => servico.CriarAsync(request)
        );
    }
}
