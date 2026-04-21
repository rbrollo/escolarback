namespace Escolar.Application.Dtos.Shared;

public class ConsultaPaginadaDto
{
    public int? Pagina { get; set; }
    public int? TamanhoPagina { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }

    public int ObterPagina() => Pagina ?? Page ?? 1;

    public int ObterTamanhoPagina() => TamanhoPagina ?? PageSize ?? 10;
}
