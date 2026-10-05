using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class ServicoEstoqueTests
{
    [Fact]
    public async Task DeveConsultarSaldoDoMaterialNaUnidade()
    {
        var repositorioUnidade =
            new RepositorioUnidadeFake();

        var repositorioMaterial =
            new RepositorioMaterialFake();

        var repositorioMovimentacao =
            new RepositorioMovimentacaoEstoqueFake();

        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        await repositorioUnidade.AdicionarAsync(unidade);
        await repositorioMaterial.AdicionarAsync(material);

        await repositorioMovimentacao.AdicionarAsync(
            new MovimentacaoEstoque(
                unidade.Id,
                material.Id,
                TipoMovimentacao.Entrada,
                20
            )
        );

        await repositorioMovimentacao.AdicionarAsync(
            new MovimentacaoEstoque(
                unidade.Id,
                material.Id,
                TipoMovimentacao.Saida,
                7
            )
        );

        var servico = new ServicoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var resultado = await servico.ConsultarSaldoAsync(
            unidade.Id,
            material.Id
        );

        Assert.Equal(unidade.Id, resultado.UnidadeId);
        Assert.Equal("Prédio A", resultado.UnidadeNome);

        Assert.Equal(material.Id, resultado.MaterialId);
        Assert.Equal("Álcool 70%", resultado.MaterialNome);

        Assert.Equal(13, resultado.Saldo);
    }

    [Fact]
    public async Task DeveRetornarSaldoZeroSemMovimentacoes()
    {
        var repositorioUnidade =
            new RepositorioUnidadeFake();

        var repositorioMaterial =
            new RepositorioMaterialFake();

        var repositorioMovimentacao =
            new RepositorioMovimentacaoEstoqueFake();

        var unidade = new Unidade("Prédio A");

        var material = new Material(
            "Detergente",
            UnidadeMedida.Litro
        );

        await repositorioUnidade.AdicionarAsync(unidade);
        await repositorioMaterial.AdicionarAsync(material);

        var servico = new ServicoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var resultado = await servico.ConsultarSaldoAsync(
            unidade.Id,
            material.Id
        );

        Assert.Equal(0, resultado.Saldo);
    }

    [Fact]
    public async Task NaoDeveConsultarSaldoDeUnidadeInexistente()
    {
        var repositorioUnidade =
            new RepositorioUnidadeFake();

        var repositorioMaterial =
            new RepositorioMaterialFake();

        var repositorioMovimentacao =
            new RepositorioMovimentacaoEstoqueFake();

        var material = new Material(
            "Álcool 70%",
            UnidadeMedida.Litro
        );

        await repositorioMaterial.AdicionarAsync(material);

        var servico = new ServicoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servico.ConsultarSaldoAsync(
                Guid.NewGuid(),
                material.Id
            )
        );
    }

    [Fact]
    public async Task NaoDeveConsultarSaldoDeMaterialInexistente()
    {
        var repositorioUnidade =
            new RepositorioUnidadeFake();

        var repositorioMaterial =
            new RepositorioMaterialFake();

        var repositorioMovimentacao =
            new RepositorioMovimentacaoEstoqueFake();

        var unidade = new Unidade("Prédio A");

        await repositorioUnidade.AdicionarAsync(unidade);

        var servico = new ServicoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => servico.ConsultarSaldoAsync(
                unidade.Id,
                Guid.NewGuid()
            )
        );
    }
}
