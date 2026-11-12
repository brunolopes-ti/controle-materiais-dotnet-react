using ControleMateriais.Api.TratamentoErros;
using ControleMateriais.Domain.Excecoes;
using Microsoft.AspNetCore.Http;

namespace ControleMateriais.Tests.Api;

public class ApiExceptionHandlerTests
{
    [Fact]
    public void DeveMapearExcecoesDoProjetoParaStatusHttp()
    {
        Assert.Equal(
            StatusCodes.Status400BadRequest,
            ApiExceptionHandler.Mapear(
                new RegraNegocioException("x")).StatusCode
        );
        Assert.Equal(
            StatusCodes.Status404NotFound,
            ApiExceptionHandler.Mapear(
                new RecursoNaoEncontradoException("x")).StatusCode
        );
        Assert.Equal(
            StatusCodes.Status409Conflict,
            ApiExceptionHandler.Mapear(
                new ConflitoException("x")).StatusCode
        );
        Assert.Equal(
            StatusCodes.Status401Unauthorized,
            ApiExceptionHandler.Mapear(
                new NaoAutorizadoException("x")).StatusCode
        );
        Assert.Equal(
            StatusCodes.Status400BadRequest,
            ApiExceptionHandler.Mapear(
                new ArgumentException("x")).StatusCode
        );
    }

    [Fact]
    public void DeveDevolverMensagemDaRegraDeNegocioAoCliente()
    {
        var resultado =
            ApiExceptionHandler.Mapear(
                new RegraNegocioException(
                    "Estoque insuficiente para realizar a saída."
                )
            );

        Assert.Equal(
            "Estoque insuficiente para realizar a saída.",
            resultado.Detail
        );
    }

    [Fact]
    public void NaoDeveExporMensagemDeExcecoesGenericasDoDotNet()
    {
        // Exceções genéricas podem vir do EF Core ou do Npgsql e
        // conter detalhes internos (nomes de tabelas, conexão etc.).
        var excecoes = new Exception[]
        {
            new InvalidOperationException("detalhe interno do EF"),
            new KeyNotFoundException("chave interna"),
            new UnauthorizedAccessException("caminho de arquivo")
        };

        foreach (var excecao in excecoes)
        {
            var resultado = ApiExceptionHandler.Mapear(excecao);

            Assert.Equal(
                StatusCodes.Status500InternalServerError,
                resultado.StatusCode
            );
            Assert.Equal(
                "Ocorreu um erro inesperado.",
                resultado.Detail
            );
        }
    }
}
