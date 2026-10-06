using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;

namespace ControleMateriais.Tests.Application;

public class ServicoUsuarioTests
{
    [Fact]
    public async Task DeveCriarUsuarioComSenhaHasheada()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var servico =
            new ServicoUsuario(
                repositorio,
                hashSenha
            );

        var request =
            new CriarUsuarioRequest
            {
                Nome = "Bruno Lopes",
                Email = "BRUNO@EXEMPLO.COM",
                Senha = "MinhaSenha123!"
            };

        var resultado =
            await servico.CriarAsync(request);

        var usuarioSalvo =
            await repositorio.ObterPorEmailAsync(
                "bruno@exemplo.com"
            );

        Assert.NotNull(usuarioSalvo);

        Assert.Equal(
            "Bruno Lopes",
            resultado.Nome
        );

        Assert.Equal(
            "bruno@exemplo.com",
            resultado.Email
        );

        Assert.Equal(
            "HASH::MinhaSenha123!",
            usuarioSalvo.SenhaHash
        );

        Assert.NotEqual(
            request.Senha,
            usuarioSalvo.SenhaHash
        );
    }

    [Fact]
    public async Task NaoDeveCriarUsuarioComEmailDuplicado()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var servico =
            new ServicoUsuario(
                repositorio,
                hashSenha
            );

        await servico.CriarAsync(
            new CriarUsuarioRequest
            {
                Nome = "Bruno",
                Email = "bruno@exemplo.com",
                Senha = "MinhaSenha123!"
            }
        );

        var excecao =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => servico.CriarAsync(
                    new CriarUsuarioRequest
                    {
                        Nome = "Outro Bruno",
                        Email = "BRUNO@EXEMPLO.COM",
                        Senha = "OutraSenha123!"
                    }
                )
            );

        Assert.Equal(
            "Já existe um usuário cadastrado com este e-mail.",
            excecao.Message
        );
    }

    [Fact]
    public async Task NaoDeveCriarUsuarioComSenhaMenorQueOitoCaracteres()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var servico =
            new ServicoUsuario(
                repositorio,
                hashSenha
            );

        var request =
            new CriarUsuarioRequest
            {
                Nome = "Bruno Lopes",
                Email = "bruno@exemplo.com",
                Senha = "1234567"
            };

        var excecao =
            await Assert.ThrowsAsync<ArgumentException>(
                () => servico.CriarAsync(request)
            );

        Assert.Contains(
            "A senha deve possuir pelo menos 8 caracteres.",
            excecao.Message
        );
    }
}
