using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Application.Contratos;

public interface IGeradorToken
{
    string Gerar(Usuario usuario);
}
