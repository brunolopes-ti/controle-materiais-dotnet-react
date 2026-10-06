namespace ControleMateriais.Application.Contratos;

public interface IServicoHashSenha
{
    string GerarHash(string senha);

    bool Verificar(
        string senhaHash,
        string senha
    );
}
