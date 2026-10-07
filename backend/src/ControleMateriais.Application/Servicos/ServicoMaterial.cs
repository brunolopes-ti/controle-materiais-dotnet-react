using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Servicos;

public class ServicoMaterial
{
    private readonly IRepositorioMaterial _repositorioMaterial;

    public ServicoMaterial(
        IRepositorioMaterial repositorioMaterial)
    {
        _repositorioMaterial = repositorioMaterial;
    }

    public async Task<MaterialResponse> CriarAsync(
        CriarMaterialRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var material =
            new Material(
                request.Nome,
                request.UnidadeMedida
            );

        await _repositorioMaterial.AdicionarAsync(
            material
        );

        return Mapear(material);
    }

    public async Task<IReadOnlyCollection<MaterialResponse>>
        ListarAsync()
    {
        var materiais =
            await _repositorioMaterial.ListarAsync();

        return materiais
            .Select(Mapear)
            .ToList();
    }

    private static MaterialResponse Mapear(
        Material material)
    {
        return new MaterialResponse
        {
            Id = material.Id,
            Nome = material.Nome,
            UnidadeMedida = material.UnidadeMedida,
            Ativo = material.Ativo
        };
    }
}
