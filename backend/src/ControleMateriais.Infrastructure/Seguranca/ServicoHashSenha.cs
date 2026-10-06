using ControleMateriais.Application.Contratos;
using Microsoft.AspNetCore.Identity;

namespace ControleMateriais.Infrastructure.Seguranca;

public class ServicoHashSenha : IServicoHashSenha
{
    private readonly PasswordHasher<object> _passwordHasher =
        new();

    private readonly object _usuario =
        new();

    public string GerarHash(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
        {
            throw new ArgumentException(
                "A senha é obrigatória.",
                nameof(senha)
            );
        }

        return _passwordHasher.HashPassword(
            _usuario,
            senha
        );
    }

    public bool Verificar(
        string senhaHash,
        string senha)
    {
        if (
            string.IsNullOrWhiteSpace(senhaHash) ||
            string.IsNullOrWhiteSpace(senha)
        )
        {
            return false;
        }

        var resultado =
            _passwordHasher.VerifyHashedPassword(
                _usuario,
                senhaHash,
                senha
            );

        return resultado !=
            PasswordVerificationResult.Failed;
    }
}
