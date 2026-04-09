using Escolar.Application.Dtos;
using Escolar.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Escolar.Api.Controllers;

public class AlunosController(IServicoAluno servicoAluno)
    : ControladorCrudBase<AlunoRespostaDto, AlunoCriarDto, AlunoAtualizarDto>(servicoAluno)
{
    [HttpGet]
    public override async Task<ActionResult<RespostaPaginadaDto<AlunoRespostaDto>>> Listar(
        [FromQuery] int? pagina = null,
        [FromQuery] int? tamanhoPagina = null,
        [FromQuery(Name = "page")] int? page = null,
        [FromQuery(Name = "pageSize")] int? pageSize = null)
    {
        var consulta = new AlunoConsultaDto
        {
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            Page = page,
            PageSize = pageSize,
            Nome = Request.Query["nome"]
        };

        var registros = await servicoAluno.ListarAsync(consulta);
        return Ok(registros);
    }
}
