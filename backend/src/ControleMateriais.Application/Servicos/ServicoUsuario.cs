using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;
using ControleMateriais.Domain.Entidades;
using ControleMateriais.Domain.Excecoes;

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

    /// <summary>
    /// Cria um usuário.
    ///
    /// Regra de acesso: sem autenticação, só é possível criar o
    /// primeiro usuário do sistema (para não ficar impossível
    /// começar a usar a API). A partir daí, só um usuário
    /// autenticado pode cadastrar outros.
    /// </summary>
    public async Task<UsuarioResponse> CriarAsync(
        CriarUsuarioRequest request,
        bool solicitanteAutenticado)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (
            !solicitanteAutenticado &&
            await _repositorioUsuario.ExisteAlgumAsync()
        )
        {
            throw new NaoAutorizadoException(
                "É necessário estar autenticado para cadastrar usuários."
            );
        }

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
            throw new ConflitoException(
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
