using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Route("api/movimentacoes")]
public class MovimentacoesController : ControllerBase
{
    private readonly ServicoMovimentacaoEstoque _servicoMovimentacao;

    public MovimentacoesController(
        ServicoMovimentacaoEstoque servicoMovimentacao)
    {
        _servicoMovimentacao = servicoMovimentacao;
    }

    [HttpPost]
    public async Task<ActionResult<MovimentacaoResponse>> Registrar(
        RegistrarMovimentacaoRequest request)
    {
        var movimentacao =
            await _servicoMovimentacao.RegistrarAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            movimentacao
        );
    }
}
