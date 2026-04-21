using Microsoft.AspNetCore.Mvc;
using Escolar.Application.Dtos.Shared;
using Escolar.Application.Exceptions;
using Escolar.Application.Services.Shared;

namespace Escolar.Api.Controllers.Shared;

[ApiController]
[Route("api/[controller]")]
public abstract class ControllerCrudBase<TLeituraDto, TCriarDto, TAtualizarDto> : ControllerBase
    where TLeituraDto : class, IRespostaComId
{
    private readonly IServiceCrud<TLeituraDto, TCriarDto, TAtualizarDto> _serviceCrud;

    protected ControllerCrudBase(IServiceCrud<TLeituraDto, TCriarDto, TAtualizarDto> serviceCrud)
    {
        _serviceCrud = serviceCrud;
    }

    [HttpGet]
    public virtual async Task<ActionResult<RespostaPaginadaDto<TLeituraDto>>> Listar(
        [FromQuery] int? pagina = null,
        [FromQuery] int? tamanhoPagina = null,
        [FromQuery(Name = "page")] int? page = null,
        [FromQuery(Name = "pageSize")] int? pageSize = null)
    {
        var consulta = new ConsultaPaginadaDto
        {
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            Page = page,
            PageSize = pageSize
        };

        var registros = await _serviceCrud.ListarAsync(consulta);
        return Ok(registros);
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult<TLeituraDto>> ObterPorId(Guid id)
    {
        var registro = await _serviceCrud.ObterPorIdAsync(id);
        if (registro is null)
        {
            return NotFound("Registro nao encontrado.");
        }

        return Ok(registro);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TLeituraDto>> Criar([FromBody] TCriarDto dto)
    {
        try
        {
            var registroCriado = await _serviceCrud.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = registroCriado.Id }, registroCriado);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public virtual async Task<ActionResult<TLeituraDto>> Atualizar(Guid id, [FromBody] TAtualizarDto dto)
    {
        try
        {
            var registroAtualizado = await _serviceCrud.AtualizarAsync(id, dto);
            if (registroAtualizado is null)
            {
                return NotFound("Registro nao encontrado.");
            }

            return Ok(registroAtualizado);
        }
        catch (ValidacaoException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public virtual async Task<IActionResult> Remover(Guid id)
    {
        var removido = await _serviceCrud.RemoverAsync(id);
        if (!removido)
        {
            return NotFound("Registro nao encontrado.");
        }

        return NoContent();
    }
}
