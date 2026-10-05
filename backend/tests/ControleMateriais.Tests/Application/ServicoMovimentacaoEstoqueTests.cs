using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Tests.Application;

public class ServicoMovimentacaoEstoqueTests
{
    [Fact]
    public async Task DeveRegistrarEntradaDeEstoque()
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

        var servico = new ServicoMovimentacaoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var request = new RegistrarMovimentacaoRequest
        {
            UnidadeId = unidade.Id,
            MaterialId = material.Id,
            Tipo = TipoMovimentacao.Entrada,
            Quantidade = 10
        };

        var resultado =
            await servico.RegistrarAsync(request);

        Assert.NotEqual(Guid.Empty, resultado.Id);
        Assert.Equal(TipoMovimentacao.Entrada, resultado.Tipo);
        Assert.Equal(10, resultado.Quantidade);
        Assert.Equal(10, resultado.SaldoAtual);
    }

    [Fact]
    public async Task DeveRegistrarSaidaComEstoqueSuficiente()
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
                10
            )
        );

        var servico = new ServicoMovimentacaoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var request = new RegistrarMovimentacaoRequest
        {
            UnidadeId = unidade.Id,
            MaterialId = material.Id,
            Tipo = TipoMovimentacao.Saida,
            Quantidade = 3
        };

        var resultado =
            await servico.RegistrarAsync(request);

        Assert.Equal(TipoMovimentacao.Saida, resultado.Tipo);
        Assert.Equal(3, resultado.Quantidade);
        Assert.Equal(7, resultado.SaldoAtual);
    }

    [Fact]
    public async Task NaoDeveRegistrarSaidaSemEstoqueSuficiente()
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

        var servico = new ServicoMovimentacaoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var request = new RegistrarMovimentacaoRequest
        {
            UnidadeId = unidade.Id,
            MaterialId = material.Id,
            Tipo = TipoMovimentacao.Saida,
            Quantidade = 5
        };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => servico.RegistrarAsync(request)
        );
    }

    [Fact]
    public async Task NaoDeveRegistrarMovimentacaoParaUnidadeInexistente()
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

        var servico = new ServicoMovimentacaoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var request = new RegistrarMovimentacaoRequest
        {
            UnidadeId = Guid.NewGuid(),
            MaterialId = material.Id,
            Tipo = TipoMovimentacao.Entrada,
            Quantidade = 10
        };

        var excecao =
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => servico.RegistrarAsync(request)
            );

        Assert.Equal(
            "Unidade não encontrada.",
            excecao.Message
        );
    }

    [Fact]
    public async Task NaoDeveRegistrarMovimentacaoParaMaterialInexistente()
    {
        var repositorioUnidade =
            new RepositorioUnidadeFake();

        var repositorioMaterial =
            new RepositorioMaterialFake();

        var repositorioMovimentacao =
            new RepositorioMovimentacaoEstoqueFake();

        var unidade = new Unidade("Prédio A");

        await repositorioUnidade.AdicionarAsync(unidade);

        var servico = new ServicoMovimentacaoEstoque(
            repositorioUnidade,
            repositorioMaterial,
            repositorioMovimentacao
        );

        var request = new RegistrarMovimentacaoRequest
        {
            UnidadeId = unidade.Id,
            MaterialId = Guid.NewGuid(),
            Tipo = TipoMovimentacao.Entrada,
            Quantidade = 10
        };

        var excecao =
            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => servico.RegistrarAsync(request)
            );

        Assert.Equal(
            "Material não encontrado.",
            excecao.Message
        );
    }
}
