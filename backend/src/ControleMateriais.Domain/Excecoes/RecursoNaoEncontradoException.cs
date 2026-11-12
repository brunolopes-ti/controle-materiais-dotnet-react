namespace ControleMateriais.Domain.Excecoes;

/// <summary>
/// Recurso solicitado não existe (ex.: unidade ou material inexistente). A API responde 404.
/// </summary>
public class RecursoNaoEncontradoException : Exception
{
    public RecursoNaoEncontradoException(string mensagem)
        : base(mensagem)
    {
    }
}
