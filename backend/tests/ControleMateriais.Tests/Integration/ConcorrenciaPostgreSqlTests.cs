using ControleMateriais.Application.DTOs;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Enums;
using ControleMateriais.Domain.Excecoes;
using ControleMateriais.Infrastructure.Persistencia;
using ControleMateriais.Infrastructure.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace ControleMateriais.Tests.Integration;

/// <summary>
/// Testes que precisam de várias conexões ao mesmo tempo. Por isso
/// não usam uma transação com rollback: os dados são gravados de
/// verdade e removidos no final de cada teste.
/// </summary>
public class ConcorrenciaPostgreSqlTests
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING")
        ?? string.Empty;

    [IntegrationFact]
    public async Task SaidasSimultaneasNaoDevemDeixarEstoqueNegativo()
    {
        Guid unidadeId;
        Guid materialId;

        await using (var contexto = CriarContexto())
        {
            var unidade = new Unidade("Prédio Concorrência");
            var material = new Material(
                "Material Concorrência",
                UnidadeMedida.Unidade
            );

            await new RepositorioUnidade(contexto).AdicionarAsync(unidade);
            await new RepositorioMaterial(contexto).AdicionarAsync(material);

            unidadeId = unidade.Id;
            materialId = material.Id;
        }

        try
        {
            await RegistrarAsync(
                unidadeId,
                materialId,
                TipoMovimentacao.Entrada,
                10m
            );

            // Várias rodadas aumentam a chance de as duas saídas
            // realmente se sobreporem no banco.
            for (var rodada = 0; rodada < 5; rodada++)
            {
                var saidas = Enumerable
                    .Range(0, 2)
                    .Select(_ => TentarSaidaAsync(unidadeId, materialId, 7m))
                    .ToArray();

                var resultados = await Task.WhenAll(saidas);

                if (rodada == 0)
                {
                    // Saldo 10: só uma das duas saídas de 7 pode passar.
                    Assert.Equal(1, resultados.Count(sucesso => sucesso));
                }

                var saldo = await ConsultarSaldoAsync(unidadeId, materialId);

                Assert.True(
                    saldo >= 0,
                    $"Saldo ficou negativo na rodada {rodada}: {saldo}"
                );

                // Repõe o estoque para a próxima rodada.
                await RegistrarAsync(
                    unidadeId,
                    materialId,
                    TipoMovimentacao.Entrada,
                    10m - saldo
                );
            }
        }
        finally
        {
            await LimparAsync(unidadeId, materialId);
        }
    }

    [IntegrationFact]
    public async Task EmailDuplicadoGravadoDiretamenteDeveGerarConflito()
    {
        var email = $"conflito-{Guid.NewGuid():N}@exemplo.com";

        try
        {
            await using (var contexto = CriarContexto())
            {
                await new RepositorioUsuario(contexto).AdicionarAsync(
                    new Usuario("Primeiro", email, new string('h', 60))
                );
            }

            // Simula o segundo de dois cadastros simultâneos: ele não
            // passa pela verificação do serviço, mas o índice único
            // do banco barra e o repositório traduz para conflito.
            await using (var contexto = CriarContexto())
            {
                await Assert.ThrowsAsync<ConflitoException>(
                    () => new RepositorioUsuario(contexto).AdicionarAsync(
                        new Usuario("Segundo", email, new string('h', 60))
                    )
                );
            }
        }
        finally
        {
            await using var contexto = CriarContexto();

            await contexto.Usuarios
                .Where(usuario => usuario.Email == email)
                .ExecuteDeleteAsync();
        }
    }

    private static ControleMateriaisDbContext CriarContexto()
    {
        var options =
            new DbContextOptionsBuilder<ControleMateriaisDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

        return new ControleMateriaisDbContext(options);
    }

    private static async Task RegistrarAsync(
        Guid unidadeId,
        Guid materialId,
        TipoMovimentacao tipo,
        decimal quantidade)
    {
        if (quantidade <= 0)
        {
            return;
        }

        await using var contexto = CriarContexto();

        await CriarServico(contexto).RegistrarAsync(
            new RegistrarMovimentacaoRequest
            {
                UnidadeId = unidadeId,
                MaterialId = materialId,
                Tipo = tipo,
                Quantidade = quantidade
            }
        );
    }

    private static async Task<bool> TentarSaidaAsync(
        Guid unidadeId,
        Guid materialId,
        decimal quantidade)
    {
        try
        {
            await RegistrarAsync(
                unidadeId,
                materialId,
                TipoMovimentacao.Saida,
                quantidade
            );

            return true;
        }
        catch (RegraNegocioException)
        {
            return false;
        }
    }

    private static async Task<decimal> ConsultarSaldoAsync(
        Guid unidadeId,
        Guid materialId)
    {
        await using var contexto = CriarContexto();

        var servico = new ServicoEstoque(
            new RepositorioUnidade(contexto),
            new RepositorioMaterial(contexto),
            new RepositorioMovimentacaoEstoque(contexto)
        );

        var saldo = await servico.ConsultarSaldoAsync(unidadeId, materialId);

        return saldo.Saldo;
    }

    private static ServicoMovimentacaoEstoque CriarServico(
        ControleMateriaisDbContext contexto)
    {
        return new ServicoMovimentacaoEstoque(
            new RepositorioUnidade(contexto),
            new RepositorioMaterial(contexto),
            new RepositorioMovimentacaoEstoque(contexto),
            new BloqueioEstoque(contexto)
        );
    }

    private static async Task LimparAsync(Guid unidadeId, Guid materialId)
    {
        await using var contexto = CriarContexto();

        await contexto.MovimentacoesEstoque
            .Where(movimentacao => movimentacao.MaterialId == materialId)
            .ExecuteDeleteAsync();

        await contexto.Materiais
            .Where(material => material.Id == materialId)
            .ExecuteDeleteAsync();

        await contexto.Unidades
            .Where(unidade => unidade.Id == unidadeId)
            .ExecuteDeleteAsync();
    }
}
