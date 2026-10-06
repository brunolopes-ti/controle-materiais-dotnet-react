namespace ControleMateriais.Application.DTOs;

public class ResumoEstoqueResponse
{
    public Guid? UnidadeId { get; set; }

    public Guid? MaterialId { get; set; }

    public int TotalMovimentacoes { get; set; }

    public decimal TotalEntradas { get; set; }

    public decimal TotalSaidas { get; set; }

    public decimal Saldo { get; set; }
}
