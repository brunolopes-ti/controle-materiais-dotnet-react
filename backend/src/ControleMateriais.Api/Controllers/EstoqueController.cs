using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Route("api/estoque")]
public class EstoqueController : ControllerBase
{
    private readonly ServicoEstoque _servicoEstoque;

    public EstoqueController(
        ServicoEstoque servicoEstoque)
    {
        _servicoEstoque = servicoEstoque;
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
}
