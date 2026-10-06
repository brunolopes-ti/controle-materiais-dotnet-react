namespace ControleMateriais.Application.DTOs;

public class UsuarioResponse
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Ativo { get; set; }
}
