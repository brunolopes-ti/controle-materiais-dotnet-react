using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class RepositorioMaterialFake : IRepositorioMaterial
{
    private readonly List<Material> _materiais = new();

    public Task<Material?> ObterPorIdAsync(Guid id)
    {
        var material = _materiais
            .FirstOrDefault(material => material.Id == id);

        return Task.FromResult(material);
    }

    public Task AdicionarAsync(Material material)
    {
        _materiais.Add(material);

        return Task.CompletedTask;
    }
}
