using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Domain.Servicos;

public static class CalculadoraEstoque
{
    public static decimal CalcularSaldo(
        IEnumerable<MovimentacaoEstoque> movimentacoes)
    {
        ArgumentNullException.ThrowIfNull(movimentacoes);

        return movimentacoes.Sum(movimentacao =>
            movimentacao.Tipo == TipoMovimentacao.Entrada
                ? movimentacao.Quantidade
                : -movimentacao.Quantidade
        );
    }
}
