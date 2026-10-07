using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/unidades")]
public class UnidadesController : ControllerBase
{
    private readonly ServicoUnidade _servicoUnidade;

    public UnidadesController(
        ServicoUnidade servicoUnidade)
    {
        _servicoUnidade = servicoUnidade;
    }

    [HttpPost]
    public async Task<ActionResult<UnidadeResponse>> Criar(
        CriarUnidadeRequest request)
    {
        var unidade =
            await _servicoUnidade.CriarAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            unidade
        );
    }

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyCollection<UnidadeResponse>>>
        Listar()
    {
        var unidades =
            await _servicoUnidade.ListarAsync();

        return Ok(unidades);
    }
}
