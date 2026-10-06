using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.DTOs;

namespace ControleMateriais.Application.Servicos;

public class ServicoAutenticacao
{
    private readonly IRepositorioUsuario _repositorioUsuario;
    private readonly IServicoHashSenha _servicoHashSenha;
    private readonly IGeradorToken _geradorToken;

    public ServicoAutenticacao(
        IRepositorioUsuario repositorioUsuario,
        IServicoHashSenha servicoHashSenha,
        IGeradorToken geradorToken)
    {
        _repositorioUsuario = repositorioUsuario;
        _servicoHashSenha = servicoHashSenha;
        _geradorToken = geradorToken;
    }

    public async Task<UsuarioAutenticadoResponse> AutenticarAsync(
        LoginRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Senha)
        )
        {
            throw new UnauthorizedAccessException(
                "E-mail ou senha inválidos."
            );
        }

        var usuario =
            await _repositorioUsuario.ObterPorEmailAsync(
                request.Email
            );

        if (usuario is null)
        {
            throw new UnauthorizedAccessException(
                "E-mail ou senha inválidos."
            );
        }

        if (!usuario.Ativo)
        {
            throw new UnauthorizedAccessException(
                "E-mail ou senha inválidos."
            );
        }

        var senhaValida =
            _servicoHashSenha.Verificar(
                usuario.SenhaHash,
                request.Senha
            );

        if (!senhaValida)
        {
            throw new UnauthorizedAccessException(
                "E-mail ou senha inválidos."
            );
        }

        var token =
            _geradorToken.Gerar(usuario);

        return new UsuarioAutenticadoResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Token = token
        };
    }
}
