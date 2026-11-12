using ControleMateriais.Application.Contratos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ControleMateriais.Infrastructure.Persistencia;

/// <summary>
/// Implementa o bloqueio com uma transação e um lock pessimista
/// de linha no PostgreSQL (SELECT ... FOR UPDATE).
///
/// A linha do material fica travada até o fim da transação.
/// Uma segunda movimentação do mesmo material espera nesse SELECT
/// e só continua depois que a primeira terminar. Como a leitura
/// das movimentações acontece depois do lock, ela já enxerga a
/// movimentação gravada pela primeira operação.
/// </summary>
public class BloqueioEstoque : IBloqueioEstoque
{
    private readonly ControleMateriaisDbContext _context;

    public BloqueioEstoque(ControleMateriaisDbContext context)
    {
        _context = context;
    }

    public async Task<IOperacaoEstoqueBloqueada> BloquearAsync(
        Guid materialId)
    {
        // Se alguém (por exemplo, um teste) já abriu uma transação
        // neste DbContext, reaproveitamos essa transação. Quem abriu
        // é responsável por confirmá-la ou desfazê-la.
        var transacaoExistente =
            _context.Database.CurrentTransaction;

        var transacao =
            transacaoExistente
            ?? await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM materiais WHERE id = {materialId} FOR UPDATE"
            );
        }
        catch
        {
            if (transacaoExistente is null)
            {
                await transacao.DisposeAsync();
            }

            throw;
        }

        return new OperacaoBloqueada(
            transacao,
            controlaTransacao: transacaoExistente is null
        );
    }

    private sealed class OperacaoBloqueada
        : IOperacaoEstoqueBloqueada
    {
        private readonly IDbContextTransaction _transacao;
        private readonly bool _controlaTransacao;

        public OperacaoBloqueada(
            IDbContextTransaction transacao,
            bool controlaTransacao)
        {
            _transacao = transacao;
            _controlaTransacao = controlaTransacao;
        }

        public async Task ConfirmarAsync()
        {
            if (_controlaTransacao)
            {
                await _transacao.CommitAsync();
            }
        }

        public async ValueTask DisposeAsync()
        {
            // Descartar uma transação não confirmada desfaz as
            // alterações (rollback) e libera o lock.
            if (_controlaTransacao)
            {
                await _transacao.DisposeAsync();
            }
        }
    }
}
