using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Servicos;

namespace ControleMateriais.Application.Servicos;

public class ServicoMovimentacaoEstoque
{
    private readonly IRepositorioUnidade _repositorioUnidade;
    private readonly IRepositorioMaterial _repositorioMaterial;
    private readonly IRepositorioMovimentacaoEstoque _repositorioMovimentacao;

    public ServicoMovimentacaoEstoque(
        IRepositorioUnidade repositorioUnidade,
        IRepositorioMaterial repositorioMaterial,
        IRepositorioMovimentacaoEstoque repositorioMovimentacao)
    {
        _repositorioUnidade = repositorioUnidade;
        _repositorioMaterial = repositorioMaterial;
        _repositorioMovimentacao = repositorioMovimentacao;
    }

    public async Task<MovimentacaoResponse> RegistrarAsync(
        RegistrarMovimentacaoRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unidade = await _repositorioUnidade
            .ObterPorIdAsync(request.UnidadeId);

        if (unidade is null)
        {
            throw new KeyNotFoundException(
                "Unidade não encontrada."
            );
        }

        var material = await _repositorioMaterial
            .ObterPorIdAsync(request.MaterialId);

        if (material is null)
        {
            throw new KeyNotFoundException(
                "Material não encontrado."
            );
        }

        var movimentacoes = await _repositorioMovimentacao
            .ListarPorUnidadeEMaterialAsync(
                unidade.Id,
                material.Id
            );

        var saldoAtual =
            CalculadoraEstoque.CalcularSaldo(movimentacoes);

        ValidadorMovimentacaoEstoque.Validar(
            unidade,
            material,
            request.Tipo,
            request.Quantidade,
            saldoAtual
        );

        var movimentacao = new MovimentacaoEstoque(
            unidade.Id,
            material.Id,
            request.Tipo,
            request.Quantidade
        );

        await _repositorioMovimentacao
            .AdicionarAsync(movimentacao);

        var novoSaldo =
            request.Tipo == TipoMovimentacao.Entrada
                ? saldoAtual + request.Quantidade
                : saldoAtual - request.Quantidade;

        return new MovimentacaoResponse
        {
            Id = movimentacao.Id,
            UnidadeId = movimentacao.UnidadeId,
            MaterialId = movimentacao.MaterialId,
            Tipo = movimentacao.Tipo,
            Quantidade = movimentacao.Quantidade,
            SaldoAtual = novoSaldo,
            DataMovimentacao = movimentacao.DataMovimentacao
        };
    }
}
