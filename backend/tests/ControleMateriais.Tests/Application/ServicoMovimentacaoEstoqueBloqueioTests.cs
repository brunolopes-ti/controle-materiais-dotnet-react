using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Excecoes;

namespace ControleMateriais.Tests.Application;

public class ServicoMovimentacaoEstoqueBloqueioTests
{
    [Fact]
    public async Task DeveBloquearMaterialEConfirmarQuandoMovimentacaoForRegistrada()
    {
        var cenario = await CriarCenarioAsync();

        await cenario.Servico.RegistrarAsync(
            new RegistrarMovimentacaoRequest
            {
                UnidadeId = cenario.Unidade.Id,
                MaterialId = cenario.Material.Id,
                Tipo = TipoMovimentacao.Entrada,
                Quantidade = 5
            }
        );

        Assert.Equal(
            new[] { cenario.Material.Id },
            cenario.Bloqueio.MateriaisBloqueados
        );
        Assert.Equal(1, cenario.Bloqueio.Confirmacoes);
        Assert.Equal(1, cenario.Bloqueio.Liberacoes);
    }

    [Fact]
    public async Task NaoDeveConfirmarMasDeveLiberarQuandoEstoqueForInsuficiente()
    {
        var cenario = await CriarCenarioAsync();

        await Assert.ThrowsAsync<RegraNegocioException>(
            () => cenario.Servico.RegistrarAsync(
                new RegistrarMovimentacaoRequest
                {
                    UnidadeId = cenario.Unidade.Id,
                    MaterialId = cenario.Material.Id,
                    Tipo = TipoMovimentacao.Saida,
                    Quantidade = 1
                }
            )
        );

        Assert.Equal(0, cenario.Bloqueio.Confirmacoes);
        Assert.Equal(1, cenario.Bloqueio.Liberacoes);
        Assert.Empty(
            await cenario.RepositorioMovimentacao.ListarAsync()
        );
    }

    [Fact]
    public async Task NaoDeveBloquearQuandoMaterialNaoExistir()
    {
        var cenario = await CriarCenarioAsync();

        await Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            () => cenario.Servico.RegistrarAsync(
                new RegistrarMovimentacaoRequest
                {
                    UnidadeId = cenario.Unidade.Id,
                    MaterialId = Guid.NewGuid(),
                    Tipo = TipoMovimentacao.Entrada,
                    Quantidade = 1
                }
            )
        );

        Assert.Empty(cenario.Bloqueio.MateriaisBloqueados);
    }

    private static async Task<Cenario> CriarCenarioAsync()
    {
        var repositorioUnidade = new RepositorioUnidadeFake();
        var repositorioMaterial = new RepositorioMaterialFake();
        var repositorioMovimentacao =
            new RepositorioMovimentacaoEstoqueFake();
        var bloqueio = new BloqueioEstoqueFake();

        var unidade = new Unidade("Prédio A");
        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        await repositorioUnidade.AdicionarAsync(unidade);
        await repositorioMaterial.AdicionarAsync(material);

        var servico = new ServicoMovimentacaoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao,
            bloqueio
        );

        return new Cenario(
            servico,
            bloqueio,
            repositorioMovimentacao,
            unidade,
            material
        );
    }

    private sealed record Cenario(
        ServicoMovimentacaoEstoque Servico,
        BloqueioEstoqueFake Bloqueio,
        RepositorioMovimentacaoEstoqueFake RepositorioMovimentacao,
        Unidade Unidade,
        Material Material
    );
}
