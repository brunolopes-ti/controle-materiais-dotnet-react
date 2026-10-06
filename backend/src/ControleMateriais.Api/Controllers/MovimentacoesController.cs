using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/movimentacoes")]
public class MovimentacoesController : ControllerBase
{
    private readonly ServicoMovimentacaoEstoque _servicoMovimentacao;
    private readonly ServicoHistoricoMovimentacao _servicoHistorico;

    public MovimentacoesController(
        ServicoMovimentacaoEstoque servicoMovimentacao,
        ServicoHistoricoMovimentacao servicoHistorico)
    {
        _servicoMovimentacao = servicoMovimentacao;
        _servicoHistorico = servicoHistorico;
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

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyCollection<HistoricoMovimentacaoResponse>>>
        Listar(
            [FromQuery] Guid? unidadeId,
            [FromQuery] Guid? materialId)
    {
        var historico =
            await _servicoHistorico.ListarAsync(
                unidadeId,
                materialId
            );

        return Ok(historico);
    }
}
