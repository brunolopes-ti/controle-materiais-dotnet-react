using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
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
        await _context.MovimentacoesEstoque.AddAsync(
            movimentacao
        );

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
                movimentacao.MaterialId == materialId
            )
            .ToListAsync();
    }

    public async Task<IReadOnlyCollection<MovimentacaoEstoque>>
        ListarAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            CriarConsulta(
                unidadeId,
                materialId
            );

        return await consulta
            .OrderByDescending(
                movimentacao =>
                    movimentacao.DataMovimentacao
            )
            .ToListAsync();
    }

    public async Task<(
        IReadOnlyCollection<MovimentacaoEstoque> Itens,
        int TotalItens
    )>
        ListarPaginadoAsync(
            int pagina,
            int tamanhoPagina,
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            CriarConsulta(
                unidadeId,
                materialId
            );

        var totalItens =
            await consulta.CountAsync();

        var itens =
            await consulta
                .OrderByDescending(
                    movimentacao =>
                        movimentacao.DataMovimentacao
                )
                .Skip(
                    (pagina - 1) * tamanhoPagina
                )
                .Take(tamanhoPagina)
                .ToListAsync();

        return (
            itens,
            totalItens
        );
    }

    public async Task<(
        int TotalMovimentacoes,
        decimal TotalEntradas,
        decimal TotalSaidas
    )>
        ObterResumoAsync(
            Guid? unidadeId = null,
            Guid? materialId = null)
    {
        var consulta =
            CriarConsulta(
                unidadeId,
                materialId
            );

        var resumo =
            await consulta
                .GroupBy(_ => 1)
                .Select(grupo => new
                {
                    TotalMovimentacoes =
                        grupo.Count(),

                    TotalEntradas =
                        grupo
                            .Where(movimentacao =>
                                movimentacao.Tipo ==
                                TipoMovimentacao.Entrada
                            )
                            .Sum(movimentacao =>
                                movimentacao.Quantidade
                            ),

                    TotalSaidas =
                        grupo
                            .Where(movimentacao =>
                                movimentacao.Tipo ==
                                TipoMovimentacao.Saida
                            )
                            .Sum(movimentacao =>
                                movimentacao.Quantidade
                            )
                })
                .FirstOrDefaultAsync();

        if (resumo is null)
        {
            return (
                0,
                0m,
                0m
            );
        }

        return (
            resumo.TotalMovimentacoes,
            resumo.TotalEntradas,
            resumo.TotalSaidas
        );
    }

    private IQueryable<MovimentacaoEstoque> CriarConsulta(
        Guid? unidadeId,
        Guid? materialId)
    {
        var consulta =
            _context.MovimentacoesEstoque
                .AsNoTracking()
                .AsQueryable();

        if (unidadeId.HasValue)
        {
            consulta =
                consulta.Where(
                    movimentacao =>
                        movimentacao.UnidadeId ==
                        unidadeId.Value
                );
        }

        if (materialId.HasValue)
        {
            consulta =
                consulta.Where(
                    movimentacao =>
                        movimentacao.MaterialId ==
                        materialId.Value
                );
        }

        return consulta;
    }
}
