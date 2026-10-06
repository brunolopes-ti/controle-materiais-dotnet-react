using System.Net.Mail;

namespace ControleMateriais.Domain.Entidades;

public class Usuario
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; }

    public string Email { get; private set; }

    public string SenhaHash { get; private set; }

    public bool Ativo { get; private set; }

    public Usuario(
        string nome,
        string email,
        string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException(
                "O nome do usuário é obrigatório.",
                nameof(nome)
            );
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "O e-mail do usuário é obrigatório.",
                nameof(email)
            );
        }

        var emailNormalizado =
            email.Trim().ToLowerInvariant();

        if (
            !MailAddress.TryCreate(
                emailNormalizado,
                out var enderecoEmail
            ) ||
            enderecoEmail.Address != emailNormalizado
        )
        {
            throw new ArgumentException(
                "O e-mail do usuário é inválido.",
                nameof(email)
            );
        }

        if (string.IsNullOrWhiteSpace(senhaHash))
        {
            throw new ArgumentException(
                "O hash da senha é obrigatório.",
                nameof(senhaHash)
            );
        }

        Id = Guid.NewGuid();
        Nome = nome.Trim();
        Email = emailNormalizado;
        SenhaHash = senhaHash;
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }
}
