using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
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
}
