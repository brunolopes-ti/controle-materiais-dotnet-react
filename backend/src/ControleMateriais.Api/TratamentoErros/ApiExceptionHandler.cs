using ControleMateriais.Domain.Excecoes;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.TratamentoErros;

/// <summary>
/// Converte exceções em respostas HTTP padronizadas (ProblemDetails).
///
/// Só exceções criadas pelo próprio projeto (pasta Domain/Excecoes)
/// e ArgumentException (validação das entidades) têm a mensagem
/// devolvida ao cliente. Exceções genéricas do .NET, como
/// InvalidOperationException ou KeyNotFoundException, também são
/// lançadas por bibliotecas (EF Core, Npgsql) e podem conter detalhes
/// internos; por isso caem no 500 com mensagem genérica.
/// </summary>
public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;

    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = Mapear(exception);

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            // O cliente recebe mensagem genérica; o detalhe fica no log.
            _logger.LogError(
                exception,
                "Erro inesperado ao processar a requisição."
            );
        }

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            },
            cancellationToken
        );

        return true;
    }

    public static (int StatusCode, string Title, string Detail) Mapear(
        Exception exception)
    {
        return exception switch
        {
            NaoAutorizadoException =>
                (
                    StatusCodes.Status401Unauthorized,
                    "Não autorizado.",
                    exception.Message
                ),

            RecursoNaoEncontradoException =>
                (
                    StatusCodes.Status404NotFound,
                    "Recurso não encontrado.",
                    exception.Message
                ),

            ConflitoException =>
                (
                    StatusCodes.Status409Conflict,
                    "Conflito.",
                    exception.Message
                ),

            RegraNegocioException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Operação inválida.",
                    exception.Message
                ),

            ArgumentException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Requisição inválida.",
                    exception.Message
                ),

            _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor.",
                    "Ocorreu um erro inesperado."
                )
        };
    }
}
