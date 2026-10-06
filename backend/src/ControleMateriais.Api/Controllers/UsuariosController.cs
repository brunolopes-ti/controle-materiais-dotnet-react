using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly ServicoUsuario _servicoUsuario;

    public UsuariosController(
        ServicoUsuario servicoUsuario)
    {
        _servicoUsuario = servicoUsuario;
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioResponse>> Criar(
        CriarUsuarioRequest request)
    {
        var usuario =
            await _servicoUsuario.CriarAsync(request);

        return StatusCode(
            StatusCodes.Status201Created,
            usuario
        );
    }
}
