namespace ControleMateriais.Domain.Excecoes;

/// <summary>
/// Violação de uma regra de negócio conhecida (ex.: estoque insuficiente). A API responde 400.
/// </summary>
public class RegraNegocioException : Exception
{
    public RegraNegocioException(string mensagem)
        : base(mensagem)
    {
    }
}
