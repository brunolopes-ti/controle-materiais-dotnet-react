namespace ControleMateriais.Application.DTOs;

public class UnidadeResponse
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public bool Ativa { get; set; }
}
