using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class ServicoAutenticacaoTests
{
    [Fact]
    public async Task DeveAutenticarUsuarioComCredenciaisValidas()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var token =
            new GeradorTokenFake();

        var senha = "MinhaSenha123!";

        var usuario =
            new Usuario(
                "Bruno Lopes",
                "bruno@exemplo.com",
                hashSenha.GerarHash(senha)
            );

        await repositorio.AdicionarAsync(usuario);

        var servico =
            new ServicoAutenticacao(
                repositorio,
                hashSenha,
                token
            );

        var resultado =
            await servico.AutenticarAsync(
                new LoginRequest
                {
                    Email = "bruno@exemplo.com",
                    Senha = senha
                }
            );

        Assert.Equal(
            usuario.Id,
            resultado.Id
        );

        Assert.Equal(
            "Bruno Lopes",
            resultado.Nome
        );

        Assert.Equal(
            "bruno@exemplo.com",
            resultado.Email
        );

        Assert.Equal(
            $"TOKEN::{usuario.Id}",
            resultado.Token
        );
    }

    [Fact]
    public async Task NaoDeveAutenticarUsuarioComSenhaIncorreta()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var token =
            new GeradorTokenFake();

        var usuario =
            new Usuario(
                "Bruno Lopes",
                "bruno@exemplo.com",
                hashSenha.GerarHash(
                    "MinhaSenha123!"
                )
            );

        await repositorio.AdicionarAsync(usuario);

        var servico =
            new ServicoAutenticacao(
                repositorio,
                hashSenha,
                token
            );

        var excecao =
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => servico.AutenticarAsync(
                    new LoginRequest
                    {
                        Email = "bruno@exemplo.com",
                        Senha = "SenhaErrada123!"
                    }
                )
            );

        Assert.Equal(
            "E-mail ou senha inválidos.",
            excecao.Message
        );
    }

    [Fact]
    public async Task NaoDeveAutenticarUsuarioInexistente()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var token =
            new GeradorTokenFake();

        var servico =
            new ServicoAutenticacao(
                repositorio,
                hashSenha,
                token
            );

        var excecao =
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => servico.AutenticarAsync(
                    new LoginRequest
                    {
                        Email = "naoexiste@exemplo.com",
                        Senha = "MinhaSenha123!"
                    }
                )
            );

        Assert.Equal(
            "E-mail ou senha inválidos.",
            excecao.Message
        );
    }

    [Fact]
    public async Task NaoDeveAutenticarUsuarioInativo()
    {
        var repositorio =
            new RepositorioUsuarioFake();

        var hashSenha =
            new ServicoHashSenhaFake();

        var token =
            new GeradorTokenFake();

        var usuario =
            new Usuario(
                "Bruno Lopes",
                "bruno@exemplo.com",
                hashSenha.GerarHash(
                    "MinhaSenha123!"
                )
            );

        usuario.Desativar();

        await repositorio.AdicionarAsync(usuario);

        var servico =
            new ServicoAutenticacao(
                repositorio,
                hashSenha,
                token
            );

        var excecao =
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => servico.AutenticarAsync(
                    new LoginRequest
                    {
                        Email = "bruno@exemplo.com",
                        Senha = "MinhaSenha123!"
                    }
                )
            );

        Assert.Equal(
            "E-mail ou senha inválidos.",
            excecao.Message
        );
    }
}
