namespace ControleMateriais.Domain.Excecoes;

/// <summary>
/// Credenciais ausentes ou inválidas para a operação. A API responde 401.
/// </summary>
public class NaoAutorizadoException : Exception
{
    public NaoAutorizadoException(string mensagem)
        : base(mensagem)
    {
    }
}
