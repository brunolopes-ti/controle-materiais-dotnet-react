namespace ControleMateriais.Domain.Excecoes;

/// <summary>
/// Operação conflita com dados já existentes (ex.: e-mail já cadastrado). A API responde 409.
/// </summary>
public class ConflitoException : Exception
{
    public ConflitoException(string mensagem)
        : base(mensagem)
    {
    }
}
