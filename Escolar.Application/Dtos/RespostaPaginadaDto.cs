namespace Escolar.Application.Dtos;

public class RespostaPaginadaDto<T>
{
    public List<T> Itens { get; set; } = [];
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas { get; set; }

    public static RespostaPaginadaDto<TDestino> Criar<TOrigem, TDestino>(
        List<TOrigem> itens,
        int pagina,
        int tamanhoPagina,
        int totalItens,
        Func<TOrigem, TDestino> mapear)
    {
        return new RespostaPaginadaDto<TDestino>
        {
            Itens = itens.Select(mapear).ToList(),
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = totalItens,
            TotalPaginas = totalItens == 0 ? 0 : (int)Math.Ceiling(totalItens / (double)tamanhoPagina)
        };
    }
}
