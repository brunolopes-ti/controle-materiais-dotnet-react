using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class RepositorioUnidadeFake : IRepositorioUnidade
{
    private readonly List<Unidade> _unidades = new();

    public Task<Unidade?> ObterPorIdAsync(
        Guid id)
    {
        var unidade =
            _unidades.FirstOrDefault(
                unidade => unidade.Id == id
            );

        return Task.FromResult(unidade);
    }

    public Task<IReadOnlyCollection<Unidade>>
        ListarAsync()
    {
        IReadOnlyCollection<Unidade> resultado =
            _unidades
                .OrderBy(unidade => unidade.Nome)
                .ToList();

        return Task.FromResult(resultado);
    }

    public Task AdicionarAsync(
        Unidade unidade)
    {
        _unidades.Add(unidade);

        return Task.CompletedTask;
    }
}
