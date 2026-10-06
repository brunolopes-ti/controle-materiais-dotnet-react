using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Servicos;

public class ServicoUsuario
{
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IServicoHashSenha _servicoHashSenha;

    public ServicoUsuario(
        IRepositorioUsuario repositorioUsuario,
        IServicoHashSenha servicoHashSenha)
    {
        _repositorioUsuario = repositorioUsuario;
        _servicoHashSenha = servicoHashSenha;
    }

    public async Task<UsuarioResponse> CriarAsync(
        CriarUsuarioRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Senha))
        {
            throw new ArgumentException(
                "A senha é obrigatória.",
                nameof(request.Senha)
            );
        }

        if (request.Senha.Length < 8)
        {
            throw new ArgumentException(
                "A senha deve possuir pelo menos 8 caracteres.",
                nameof(request.Senha)
            );
        }

        var usuarioExistente =
            await _repositorioUsuario.ObterPorEmailAsync(
                request.Email
            );

        if (usuarioExistente is not null)
        {
            throw new InvalidOperationException(
                "Já existe um usuário cadastrado com este e-mail."
            );
        }

        var senhaHash =
            _servicoHashSenha.GerarHash(
                request.Senha
            );

        var usuario =
            new Usuario(
                request.Nome,
                request.Email,
                senhaHash
            );

        await _repositorioUsuario.AdicionarAsync(usuario);

        return new UsuarioResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Ativo = usuario.Ativo
        };
    }
}
