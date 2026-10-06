using ControleMateriais.Infrastructure.Seguranca;

namespace ControleMateriais.Tests.Infrastructure;

public class ServicoHashSenhaTests
{
    [Fact]
    public void DeveGerarHashDiferenteDaSenhaOriginal()
    {
        var servico = new ServicoHashSenha();

        var senha = "MinhaSenha123!";

        var hash = servico.GerarHash(senha);

        Assert.NotEqual(senha, hash);
        Assert.False(string.IsNullOrWhiteSpace(hash));
    }

    [Fact]
    public void DeveValidarSenhaCorreta()
    {
        var servico = new ServicoHashSenha();

        var senha = "MinhaSenha123!";

        var hash = servico.GerarHash(senha);

        var resultado =
            servico.Verificar(
                hash,
                senha
            );

        Assert.True(resultado);
    }

    [Fact]
    public void NaoDeveValidarSenhaIncorreta()
    {
        var servico = new ServicoHashSenha();

        var hash =
            servico.GerarHash(
                "MinhaSenha123!"
            );

        var resultado =
            servico.Verificar(
                hash,
                "SenhaErrada456!"
            );

        Assert.False(resultado);
    }
}
