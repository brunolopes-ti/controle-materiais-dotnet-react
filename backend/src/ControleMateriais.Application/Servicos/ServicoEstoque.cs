using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Servicos;

namespace ControleMateriais.Application.Servicos;

public class ServicoEstoque
{
    private readonly IRepositorioUnidade _repositorioUnidade;
    private readonly IRepositorioMaterial _repositorioMaterial;
    private readonly IRepositorioMovimentacaoEstoque _repositorioMovimentacao;

    public ServicoEstoque(
        IRepositorioUnidade repositorioUnidade,
        IRepositorioMaterial repositorioMaterial,
        IRepositorioMovimentacaoEstoque repositorioMovimentacao)
    {
        _repositorioUnidade = repositorioUnidade;
        _repositorioMaterial = repositorioMaterial;
        _repositorioMovimentacao = repositorioMovimentacao;
    }

    public async Task<SaldoEstoqueResponse> ConsultarSaldoAsync(
        Guid unidadeId,
        Guid materialId)
    {
        var unidade = await _repositorioUnidade
            .ObterPorIdAsync(unidadeId);

        if (unidade is null)
        {
            throw new KeyNotFoundException(
                "Unidade não encontrada."
            );
        }

        var material = await _repositorioMaterial
            .ObterPorIdAsync(materialId);

        if (material is null)
        {
            throw new KeyNotFoundException(
                "Material não encontrado."
            );
        }

        var movimentacoes = await _repositorioMovimentacao
            .ListarPorUnidadeEMaterialAsync(
                unidadeId,
                materialId
            );

        var saldo =
            CalculadoraEstoque.CalcularSaldo(movimentacoes);

        return new SaldoEstoqueResponse
        {
            UnidadeId = unidade.Id,
            UnidadeNome = unidade.Nome,
            MaterialId = material.Id,
            MaterialNome = material.Nome,
            Saldo = saldo
        };
    }
}
