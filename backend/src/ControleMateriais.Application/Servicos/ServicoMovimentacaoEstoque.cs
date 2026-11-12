using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Excecoes;
using ControleMateriais.Domain.Servicos;

namespace ControleMateriais.Application.Servicos;

public class ServicoMovimentacaoEstoque
{
    private readonly IRepositorioUnidade _repositorioUnidade;
    private readonly IRepositorioMaterial _repositorioMaterial;
    private readonly IRepositorioMovimentacaoEstoque _repositorioMovimentacao;
    private readonly IBloqueioEstoque _bloqueioEstoque;

    public ServicoMovimentacaoEstoque(
        IRepositorioUnidade repositorioUnidade,
        IRepositorioMaterial repositorioMaterial,
        IRepositorioMovimentacaoEstoque repositorioMovimentacao,
        IBloqueioEstoque bloqueioEstoque)
    {
        _repositorioUnidade = repositorioUnidade;
        _repositorioMaterial = repositorioMaterial;
        _repositorioMovimentacao = repositorioMovimentacao;
        _bloqueioEstoque = bloqueioEstoque;
    }

    public async Task<MovimentacaoResponse> RegistrarAsync(
        RegistrarMovimentacaoRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unidade = await _repositorioUnidade
            .ObterPorIdAsync(request.UnidadeId);

        if (unidade is null)
        {
            throw new RecursoNaoEncontradoException(
                "Unidade não encontrada."
            );
        }

        var material = await _repositorioMaterial
            .ObterPorIdAsync(request.MaterialId);

        if (material is null)
        {
            throw new RecursoNaoEncontradoException(
                "Material não encontrado."
            );
        }

        // Daqui até a confirmação, nenhuma outra movimentação do
        // mesmo material consegue ler o saldo. Isso impede que duas
        // saídas simultâneas usem o mesmo saldo antigo.
        await using var operacao =
            await _bloqueioEstoque.BloquearAsync(material.Id);

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

        // Só confirma se tudo acima deu certo. Se a validação lançar
        // exceção, o "await using" descarta a operação sem confirmar.
        await operacao.ConfirmarAsync();

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
