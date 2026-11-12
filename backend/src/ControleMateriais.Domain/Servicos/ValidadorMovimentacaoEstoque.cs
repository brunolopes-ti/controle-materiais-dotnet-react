using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Excecoes;

namespace ControleMateriais.Domain.Servicos;

public static class ValidadorMovimentacaoEstoque
{
    public static void Validar(
        Unidade unidade,
        Material material,
        TipoMovimentacao tipo,
        decimal quantidade,
        decimal saldoAtual)
    {
        ArgumentNullException.ThrowIfNull(unidade);
        ArgumentNullException.ThrowIfNull(material);

        if (!unidade.Ativa)
        {
            throw new RegraNegocioException(
                "Não é possível movimentar estoque de uma unidade inativa."
            );
        }

        if (!material.Ativo)
        {
            throw new RegraNegocioException(
                "Não é possível movimentar um material inativo."
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

        if (
            tipo == TipoMovimentacao.Saida &&
            quantidade > saldoAtual
        )
        {
            throw new RegraNegocioException(
                "Estoque insuficiente para realizar a saída."
            );
        }
    }
}
