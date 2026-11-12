using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Excecoes;

namespace ControleMateriais.Tests.Application;

public class ServicoUsuarioPermissaoTests
{
    [Fact]
    public async Task DevePermitirCriarPrimeiroUsuarioSemAutenticacao()
    {
        var (servico, repositorio) = CriarServico();

        await servico.CriarAsync(
            NovoRequest("admin@exemplo.com"),
            solicitanteAutenticado: false
        );

        Assert.True(await repositorio.ExisteAlgumAsync());
    }

    [Fact]
    public async Task NaoDevePermitirCadastroAnonimoQuandoJaExisteUsuario()
    {
        var (servico, repositorio) = CriarServico();

        await servico.CriarAsync(
            NovoRequest("admin@exemplo.com"),
            solicitanteAutenticado: false
        );

        var excecao =
            await Assert.ThrowsAsync<NaoAutorizadoException>(
                () => servico.CriarAsync(
                    NovoRequest("intruso@exemplo.com"),
                    solicitanteAutenticado: false
                )
            );

        Assert.Equal(
            "É necessário estar autenticado para cadastrar usuários.",
            excecao.Message
        );
        Assert.Null(
            await repositorio.ObterPorEmailAsync("intruso@exemplo.com")
        );
    }

    [Fact]
    public async Task DevePermitirUsuarioAutenticadoCadastrarOutros()
    {
        var (servico, repositorio) = CriarServico();

        await servico.CriarAsync(
            NovoRequest("admin@exemplo.com"),
            solicitanteAutenticado: false
        );

        await servico.CriarAsync(
            NovoRequest("colega@exemplo.com"),
            solicitanteAutenticado: true
        );

        Assert.NotNull(
            await repositorio.ObterPorEmailAsync("colega@exemplo.com")
        );
    }

    private static (ServicoUsuario, RepositorioUsuarioFake) CriarServico()
    {
        var repositorio = new RepositorioUsuarioFake();

        var servico = new ServicoUsuario(
            repositorio,
            new ServicoHashSenhaFake()
        );

        return (servico, repositorio);
    }

    private static CriarUsuarioRequest NovoRequest(string email)
    {
        return new CriarUsuarioRequest
        {
            Nome = "Usuário Teste",
            Email = email,
            Senha = "SenhaForte123!"
        };
    }
}
