using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Servicos;

public class ServicoUnidade
{
    private readonly IRepositorioUnidade _repositorioUnidade;

    public ServicoUnidade(
        IRepositorioUnidade repositorioUnidade)
    {
        _repositorioUnidade = repositorioUnidade;
    }

    public async Task<UnidadeResponse> CriarAsync(
        CriarUnidadeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var unidade =
            new Unidade(request.Nome);

        await _repositorioUnidade.AdicionarAsync(
            unidade
        );

        return Mapear(unidade);
    }

    public async Task<IReadOnlyCollection<UnidadeResponse>>
        ListarAsync()
    {
        var unidades =
            await _repositorioUnidade.ListarAsync();

        return unidades
            .Select(Mapear)
            .ToList();
    }

    private static UnidadeResponse Mapear(
        Unidade unidade)
    {
        return new UnidadeResponse
        {
            Id = unidade.Id,
            Nome = unidade.Nome,
            Ativa = unidade.Ativa
        };
    }
}
