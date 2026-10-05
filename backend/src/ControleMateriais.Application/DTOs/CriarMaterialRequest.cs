using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Application.DTOs;

public class CriarMaterialRequest
{
    public string Nome { get; set; } = string.Empty;

    public UnidadeMedida UnidadeMedida { get; set; }
}
