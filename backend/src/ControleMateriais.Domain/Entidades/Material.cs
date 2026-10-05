using ControleMateriais.Domain.Enums;

namespace ControleMateriais.Domain.Entidades;

public class Material
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; }

    public UnidadeMedida UnidadeMedida { get; private set; }

    public bool Ativo { get; private set; }

    public Material(string nome, UnidadeMedida unidadeMedida)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException(
                "O nome do material é obrigatório.",
                nameof(nome)
            );
        }

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        UnidadeMedida = unidadeMedida;
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }
}
