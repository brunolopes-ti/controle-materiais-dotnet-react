using ControleMateriais.Application.Contratos;

namespace ControleMateriais.Tests.Application;

public class ServicoHashSenhaFake : IServicoHashSenha
{
    public string GerarHash(string senha)
    {
        return $"HASH::{senha}";
    }

    public bool Verificar(
        string senhaHash,
        string senha)
    {
        return senhaHash == $"HASH::{senha}";
    }
}
