namespace ControleMateriais.Application.DTOs;

public class UsuarioAutenticadoResponse
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
