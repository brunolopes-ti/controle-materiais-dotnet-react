using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class ServicoUnidadeTests
{
    [Fact]
    public async Task DeveCadastrarUnidade()
    {
        var repositorio =
            new RepositorioUnidadeFake();

        var servico =
            new ServicoUnidade(repositorio);

        var request =
            new CriarUnidadeRequest
            {
                Nome = "Prédio A"
            };

        var resultado =
            await servico.CriarAsync(request);

        Assert.NotEqual(
            Guid.Empty,
            resultado.Id
        );

        Assert.Equal(
            "Prédio A",
            resultado.Nome
        );

        Assert.True(resultado.Ativa);
    }

    [Fact]
    public async Task NaoDeveCadastrarUnidadeSemNome()
    {
        var repositorio =
            new RepositorioUnidadeFake();

        var servico =
            new ServicoUnidade(repositorio);

        var request =
            new CriarUnidadeRequest
            {
                Nome = ""
            };

        await Assert.ThrowsAsync<ArgumentException>(
            () => servico.CriarAsync(request)
        );
    }

    [Fact]
    public async Task DeveListarUnidadesEmOrdemAlfabetica()
    {
        var repositorio =
            new RepositorioUnidadeFake();

        await repositorio.AdicionarAsync(
            new Unidade("Prédio C")
        );

        await repositorio.AdicionarAsync(
            new Unidade("Prédio A")
        );

        await repositorio.AdicionarAsync(
            new Unidade("Prédio B")
        );

        var servico =
            new ServicoUnidade(repositorio);

        var resultado =
            await servico.ListarAsync();

        Assert.Equal(
            3,
            resultado.Count
        );

        Assert.Equal(
            "Prédio A",
            resultado.ElementAt(0).Nome
        );

        Assert.Equal(
            "Prédio B",
            resultado.ElementAt(1).Nome
        );

        Assert.Equal(
            "Prédio C",
            resultado.ElementAt(2).Nome
        );
    }
}
