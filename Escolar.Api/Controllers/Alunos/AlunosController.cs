using Escolar.Application.Dtos.Aluno;
using Escolar.Application.Dtos.Shared;
using Escolar.Application.Services.Aluno;
using Escolar.Api.Controllers.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Escolar.Api.Controllers.Alunos;

public class AlunosController(IServiceAluno serviceAluno)
    : ControllerCrudBase<AlunoRespostaDto, AlunoCriarDto, AlunoAtualizarDto>(serviceAluno)
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

        var registros = await serviceAluno.ListarAsync(consulta);
        return Ok(registros);
    }
}
