using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ControleMateriais.Infrastructure.Persistencia.Repositorios;

public class RepositorioMovimentacaoEstoque
    : IRepositorioMovimentacaoEstoque
{
    private readonly ControleMateriaisDbContext _context;

    public RepositorioMovimentacaoEstoque(
        ControleMateriaisDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(
        MovimentacaoEstoque movimentacao)
    {
        await _context.MovimentacoesEstoque.AddAsync(movimentacao);

        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarPorUnidadeEMaterialAsync(
            Guid unidadeId,
            Guid materialId)
    {
        return await _context.MovimentacoesEstoque
            .AsNoTracking()
            .Where(movimentacao =>
                movimentacao.UnidadeId == unidadeId &&
                movimentacao.MaterialId == materialId)
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            _context.MovimentacoesEstoque
                .AsNoTracking()
                .AsQueryable();

        if (unidadeId.HasValue)
        {
            consulta = consulta.Where(
                movimentacao =>
                    movimentacao.UnidadeId == unidadeId.Value
            );
        }

        if (materialId.HasValue)
        {
            consulta = consulta.Where(
                movimentacao =>
                    movimentacao.MaterialId == materialId.Value
            );
        }

        return await consulta
            .OrderByDescending(
                movimentacao => movimentacao.DataMovimentacao
            )
            .ToListAsync();
    }
}
