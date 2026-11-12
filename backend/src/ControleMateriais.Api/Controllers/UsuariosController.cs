using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Authorization;
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

    // AllowAnonymous deixa a requisição chegar aqui mesmo sem token,
    // para permitir o cadastro do primeiro usuário. Se houver um token
    // válido, o middleware de autenticação preenche "User" do mesmo
    // jeito. A decisão final fica no ServicoUsuario.
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsuarioResponse>> Criar(
        CriarUsuarioRequest request)
    {
        var solicitanteAutenticado =
            User.Identity?.IsAuthenticated == true;

        var usuario =
            await _servicoUsuario.CriarAsync(
                request,
                solicitanteAutenticado
            );

        return StatusCode(
            StatusCodes.Status201Created,
            usuario
        );
    }
}
