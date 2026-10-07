using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/materiais")]
public class MateriaisController : ControllerBase
{
    private readonly ServicoMaterial _servicoMaterial;

    public MateriaisController(
        ServicoMaterial servicoMaterial)
    {
        _servicoMaterial = servicoMaterial;
    }

    [HttpPost]
    public async Task<ActionResult<MaterialResponse>> Criar(
        CriarMaterialRequest request)
    {
        var material =
            await _servicoMaterial.CriarAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            material
        );
    }

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyCollection<MaterialResponse>>>
        Listar()
    {
        var materiais =
            await _servicoMaterial.ListarAsync();

        return Ok(materiais);
    }
}
