using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.TratamentoErros;

public sealed class ApiExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            UnauthorizedAccessException =>
                (
                    StatusCodes.Status401Unauthorized,
                    "Não autorizado.",
                    exception.Message
                ),

            KeyNotFoundException =>
                (
                    StatusCodes.Status404NotFound,
                    "Recurso não encontrado.",
                    exception.Message
                ),

            ArgumentException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Requisição inválida.",
                    exception.Message
                ),

            InvalidOperationException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Operação inválida.",
                    exception.Message
                ),

            _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "Erro interno do servidor.",
                    "Ocorreu um erro inesperado."
                )
        };

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
}
