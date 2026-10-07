namespace ControleMateriais.Application.DTOs;

public class HistoricoPaginadoResponse
{
    public IReadOnlyCollection<HistoricoMovimentacaoResponse>
        Itens { get; set; }
        = Array.Empty<HistoricoMovimentacaoResponse>();

    public int Pagina { get; set; }

    public int TamanhoPagina { get; set; }

    public int TotalItens { get; set; }

    public int TotalPaginas { get; set; }
}
