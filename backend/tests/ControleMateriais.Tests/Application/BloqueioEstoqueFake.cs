using ControleMateriais.Application.Contratos;

namespace ControleMateriais.Tests.Application;

/// <summary>
/// Fake que só registra o que aconteceu com a operação bloqueada,
/// para os testes verificarem se o serviço bloqueia, confirma e
/// libera nos momentos certos.
/// </summary>
public class BloqueioEstoqueFake : IBloqueioEstoque
{
    public List<Guid> MateriaisBloqueados { get; } = new();

    public int Confirmacoes { get; private set; }

    public int Liberacoes { get; private set; }

    public Task<IOperacaoEstoqueBloqueada> BloquearAsync(Guid materialId)
    {
        MateriaisBloqueados.Add(materialId);

        IOperacaoEstoqueBloqueada operacao = new OperacaoFake(this);

        return Task.FromResult(operacao);
    }

    private sealed class OperacaoFake : IOperacaoEstoqueBloqueada
    {
        private readonly BloqueioEstoqueFake _dono;

        public OperacaoFake(BloqueioEstoqueFake dono)
        {
            _dono = dono;
        }

        public Task ConfirmarAsync()
        {
            _dono.Confirmacoes++;

            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            _dono.Liberacoes++;

            return ValueTask.CompletedTask;
        }
    }
}
