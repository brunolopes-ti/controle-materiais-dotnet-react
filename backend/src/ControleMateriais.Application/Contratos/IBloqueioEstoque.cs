namespace ControleMateriais.Application.Contratos;

/// <summary>
/// Garante que duas movimentações do mesmo material não calculem
/// o saldo ao mesmo tempo.
///
/// Sem esse bloqueio, duas saídas simultâneas poderiam ler o mesmo
/// saldo, passar ambas pela validação e deixar o estoque negativo.
/// </summary>
public interface IBloqueioEstoque
{
    /// <summary>
    /// Inicia uma operação exclusiva para o material informado.
    /// Outras operações sobre o mesmo material aguardam até esta
    /// ser confirmada ou descartada.
    /// </summary>
    Task<IOperacaoEstoqueBloqueada> BloquearAsync(Guid materialId);
}

/// <summary>
/// Operação em andamento. Chame <see cref="ConfirmarAsync"/> quando
/// tudo der certo. Se a operação for descartada (Dispose) sem
/// confirmação, as alterações são desfeitas.
/// </summary>
public interface IOperacaoEstoqueBloqueada : IAsyncDisposable
{
    Task ConfirmarAsync();
}
