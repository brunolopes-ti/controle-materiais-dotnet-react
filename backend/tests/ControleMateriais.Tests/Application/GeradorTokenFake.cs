using ControleMateriais.Application.Contratos;
using ControleMateriais.Domain.Entidades;

namespace ControleMateriais.Tests.Application;

public class GeradorTokenFake : IGeradorToken
{
    public string Gerar(Usuario usuario)
    {
        return $"TOKEN::{usuario.Id}";
    }
}
