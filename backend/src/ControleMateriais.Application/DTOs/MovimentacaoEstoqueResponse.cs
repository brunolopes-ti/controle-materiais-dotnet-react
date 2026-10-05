using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Application.DTOs;

public class MovimentacaoEstoqueResponse
{
    public Guid Id { get; set; }

    public Guid UnidadeId { get; set; }

    public Guid MaterialId { get; set; }

    public TipoMovimentacao Tipo { get; set; }

    public decimal Quantidade { get; set; }

    public DateTimeOffset DataMovimentacao { get; set; }

    public decimal SaldoAtual { get; set; }
}
