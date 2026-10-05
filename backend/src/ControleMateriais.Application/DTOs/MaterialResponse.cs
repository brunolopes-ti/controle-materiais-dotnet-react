using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Application.DTOs;

public class MaterialResponse
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public UnidadeMedida UnidadeMedida { get; set; }

    public bool Ativo { get; set; }
}
