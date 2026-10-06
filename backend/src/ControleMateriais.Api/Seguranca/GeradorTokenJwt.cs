using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;
using Microsoft.IdentityModel.Tokens;

namespace ControleMateriais.Api.Seguranca;

public sealed class GeradorTokenJwt : IGeradorToken
{
    private readonly IConfiguration _configuration;

    public GeradorTokenJwt(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Gerar(Usuario usuario)
    {
        var chave =
            _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "A chave JWT não foi configurada."
            );

        var issuer =
            _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "O emissor JWT não foi configurado."
            );

        var audience =
            _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "A audiência JWT não foi configurada."
            );

        var expiracaoTexto =
            _configuration["Jwt:ExpirationMinutes"]
            ?? "120";

        if (
            !int.TryParse(
                expiracaoTexto,
                out var expiracaoMinutos
            ) ||
            expiracaoMinutos <= 0
        )
        {
            throw new InvalidOperationException(
                "O tempo de expiração do JWT é inválido."
            );
        }

        if (Encoding.UTF8.GetByteCount(chave) < 32)
        {
            throw new InvalidOperationException(
                "A chave JWT deve possuir pelo menos 32 bytes."
            );
        }

        var claims =
            new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    usuario.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Nome
                ),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    usuario.Email
                ),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                )
            };

        var chaveSeguranca =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(chave)
            );

        var credenciais =
            new SigningCredentials(
                chaveSeguranca,
                SecurityAlgorithms.HmacSha256
            );

        var token =
            new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    expiracaoMinutos
                ),
                signingCredentials: credenciais
            );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
