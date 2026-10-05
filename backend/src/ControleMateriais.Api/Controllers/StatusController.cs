using Microsoft.AspNetCore.Mvc;

namespace ControleMateriais.Api.Controllers;

[ApiController]
[Route("api/status")]
public class StatusController : ControllerBase
{
    [HttpGet]
    public IActionResult ObterStatus()
    {
        return Ok(new
        {
            status = "online",
            aplicacao = "Controle de Materiais"
        });
    }
}
