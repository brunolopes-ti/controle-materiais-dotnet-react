using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Domain.Entidades;

public class MovimentacaoEstoque
{
    public Guid Id { get; private set; }

    public Guid UnidadeId { get; private set; }

    public Guid MaterialId { get; private set; }

    public TipoMovimentacao Tipo { get; private set; }

    public decimal Quantidade { get; private set; }

    public DateTimeOffset DataMovimentacao { get; private set; }

    public MovimentacaoEstoque(
        Guid unidadeId,
        Guid materialId,
        TipoMovimentacao tipo,
        decimal quantidade)
    {
        if (unidadeId == Guid.Empty)
        {
            throw new ArgumentException(
                "A unidade é obrigatória.",
                nameof(unidadeId)
            );
        }

        if (materialId == Guid.Empty)
        {
            throw new ArgumentException(
                "O material é obrigatório.",
                nameof(materialId)
            );
        }

        if (!Enum.IsDefined(tipo))
        {
            throw new ArgumentOutOfRangeException(
                nameof(tipo),
                "Tipo de movimentação inválido."
            );
        }

        if (quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero.",
                nameof(quantidade)
            );
        }

        Id = Guid.NewGuid();
        UnidadeId = unidadeId;
        MaterialId = materialId;
        Tipo = tipo;
        Quantidade = quantidade;
        DataMovimentacao = DateTimeOffset.UtcNow;
    }
}
