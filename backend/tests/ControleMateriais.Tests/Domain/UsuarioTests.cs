using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Domain;

public class UsuarioTests
{
    [Fact]
    public void DeveCriarUsuarioComDadosValidos()
    {
        var usuario = new Usuario(
            " Bruno Lopes ",
            " BRUNO@EXEMPLO.COM ",
            "hash-da-senha"
        );

        Assert.NotEqual(
            Guid.Empty,
            usuario.Id
        );

        Assert.Equal(
            "Bruno Lopes",
            usuario.Nome
        );

        Assert.Equal(
            "bruno@exemplo.com",
            usuario.Email
        );

        Assert.Equal(
            "hash-da-senha",
            usuario.SenhaHash
        );

        Assert.True(usuario.Ativo);
    }

    [Fact]
    public void NaoDeveCriarUsuarioSemNome()
    {
        var excecao =
            Assert.Throws<ArgumentException>(
                () => new Usuario(
                    "",
                    "bruno@exemplo.com",
                    "hash-da-senha"
                )
            );

        Assert.Contains(
            "O nome do usuário é obrigatório.",
            excecao.Message
        );
    }

    [Fact]
    public void NaoDeveCriarUsuarioComEmailInvalido()
    {
        var excecao =
            Assert.Throws<ArgumentException>(
                () => new Usuario(
                    "Bruno Lopes",
                    "email-invalido",
                    "hash-da-senha"
                )
            );

        Assert.Contains(
            "O e-mail do usuário é inválido.",
            excecao.Message
        );
    }

    [Fact]
    public void NaoDeveCriarUsuarioSemHashDaSenha()
    {
        var excecao =
            Assert.Throws<ArgumentException>(
                () => new Usuario(
                    "Bruno Lopes",
                    "bruno@exemplo.com",
                    ""
                )
            );

        Assert.Contains(
            "O hash da senha é obrigatório.",
            excecao.Message
        );
    }

    [Fact]
    public void DeveDesativarUsuario()
    {
        var usuario = new Usuario(
            "Bruno Lopes",
            "bruno@exemplo.com",
            "hash-da-senha"
        );

        usuario.Desativar();

        Assert.False(usuario.Ativo);
    }
}
