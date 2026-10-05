using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Application.DTOs;

public class RegistrarMovimentacaoRequest
{
    public Guid UnidadeId { get; set; }

    public Guid MaterialId { get; set; }

    public TipoMovimentacao Tipo { get; set; }

    public decimal Quantidade { get; set; }
}
