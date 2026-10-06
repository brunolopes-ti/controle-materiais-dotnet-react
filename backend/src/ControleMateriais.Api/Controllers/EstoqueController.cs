using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/estoque")]
public class EstoqueController : ControllerBase
{
    private readonly ServicoEstoque _servicoEstoque;
    private readonly ServicoResumoEstoque _servicoResumoEstoque;

    public EstoqueController(
        ServicoEstoque servicoEstoque,
        ServicoResumoEstoque servicoResumoEstoque)
    {
        _servicoEstoque = servicoEstoque;
        _servicoResumoEstoque = servicoResumoEstoque;
    }

    [HttpGet("saldo")]
    public async Task<ActionResult<SaldoEstoqueResponse>> ConsultarSaldo(
        Guid unidadeId,
        Guid materialId)
    {
        var saldo =
            await _servicoEstoque.ConsultarSaldoAsync(
                unidadeId,
                materialId
            );

        return Ok(saldo);
    }

    [HttpGet("resumo")]
    public async Task<ActionResult<ResumoEstoqueResponse>> ConsultarResumo(
        [FromQuery] Guid? unidadeId,
        [FromQuery] Guid? materialId)
    {
        var resumo =
            await _servicoResumoEstoque.GerarAsync(
                unidadeId,
                materialId
            );

        return Ok(resumo);
    }
}
