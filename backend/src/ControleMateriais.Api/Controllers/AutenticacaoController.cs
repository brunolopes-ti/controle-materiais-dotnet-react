using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Route("api/autenticacao")]
public class AutenticacaoController : ControllerBase
{
    private readonly ServicoAutenticacao
        _servicoAutenticacao;

    public AutenticacaoController(
        ServicoAutenticacao servicoAutenticacao)
    {
        _servicoAutenticacao =
            servicoAutenticacao;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UsuarioAutenticadoResponse>>
        Login(LoginRequest request)
    {
        var resultado =
            await _servicoAutenticacao.AutenticarAsync(
                request
            );

        return Ok(resultado);
    }
}
