namespace ControleMateriais.Domain.Entidades;

public class Unidade
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; }

    public bool Ativa { get; private set; }

    public Unidade(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException(
                "O nome da unidade é obrigatório.",
                nameof(nome)
            );
        }

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Ativa = true;
    }

    public void Desativar()
    {
        Ativa = false;
    }
}
