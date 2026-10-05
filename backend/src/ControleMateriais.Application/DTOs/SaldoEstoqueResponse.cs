namespace ControleMateriais.Application.DTOs;

public class SaldoEstoqueResponse
{
    public Guid UnidadeId { get; set; }

    public string UnidadeNome { get; set; } = string.Empty;

    public Guid MaterialId { get; set; }

    public string MaterialNome { get; set; } = string.Empty;

    public decimal Saldo { get; set; }
}
