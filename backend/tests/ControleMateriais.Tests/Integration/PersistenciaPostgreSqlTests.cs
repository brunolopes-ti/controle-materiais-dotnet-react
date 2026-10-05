using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Infrastructure.Persistencia;
using ControleMateriais.Infrastructure.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ControleMateriais.Tests.Integration;

public class PersistenciaPostgreSqlTests
{
    [IntegrationFact]
    public async Task DevePersistirMovimentacoesECalcularSaldoNoPostgreSql()
    {
        var connectionString =
            Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")!;

        var options =
            new DbContextOptionsBuilder<ControleMateriaisDbContext>()
                .UseNpgsql(connectionString)
                .Options;

        await using var context =
            new ControleMateriaisDbContext(options);

        await using var transaction =
            await context.Database.BeginTransactionAsync();

        try
        {
            var repositorioUnidade =
                new RepositorioUnidade(context);

            var repositorioMaterial =
                new RepositorioMaterial(context);

            var repositorioMovimentacao =
                new RepositorioMovimentacaoEstoque(context);

            var servicoUnidade =
                new ServicoUnidade(repositorioUnidade);

            var servicoMaterial =
                new ServicoMaterial(repositorioMaterial);

            var servicoMovimentacao =
                new ServicoMovimentacaoEstoque(
                    repositorioUnidade,
                    repositorioMaterial,
                    repositorioMovimentacao
                );

            var servicoEstoque =
                new ServicoEstoque(
                    repositorioUnidade,
                    repositorioMaterial,
                    repositorioMovimentacao
                );

            var unidade = await servicoUnidade.CriarAsync(
                new CriarUnidadeRequest
                {
                    Nome = "Prédio Teste Integração"
                }
            );

            var material = await servicoMaterial.CriarAsync(
                new CriarMaterialRequest
                {
                    Nome = "Álcool 70%",
                    UnidadeMedida = UnidadeMedida.Litro
                }
            );

            await servicoMovimentacao.RegistrarAsync(
                new RegistrarMovimentacaoRequest
                {
                    UnidadeId = unidade.Id,
                    MaterialId = material.Id,
                    Tipo = TipoMovimentacao.Entrada,
                    Quantidade = 20m
                }
            );

            await servicoMovimentacao.RegistrarAsync(
                new RegistrarMovimentacaoRequest
                {
                    UnidadeId = unidade.Id,
                    MaterialId = material.Id,
                    Tipo = TipoMovimentacao.Saida,
                    Quantidade = 7m
                }
            );

            var saldo = await servicoEstoque.ConsultarSaldoAsync(
                unidade.Id,
                material.Id
            );

            var movimentacoes =
                await repositorioMovimentacao
                    .ListarPorUnidadeEMaterialAsync(
                        unidade.Id,
                        material.Id
                    );

            Assert.Equal(13m, saldo.Saldo);
            Assert.Equal(2, movimentacoes.Count);
        }
        finally
        {
            await transaction.RollbackAsync();
        }
    }
}

public sealed class IntegrationFactAttribute : FactAttribute
{
    public IntegrationFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(
            Environment.GetEnvironmentVariable(
                "TEST_CONNECTION_STRING")))
        {
            Skip =
                "TEST_CONNECTION_STRING não configurada.";
        }
    }
}
