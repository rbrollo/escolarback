using Microsoft.AspNetCore.Mvc;
using Escolar.Application.Dtos;
using Escolar.Application.Exceptions;
using Escolar.Application.Services;

namespace Escolar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ControladorCrudBase<TLeituraDto, TCriarDto, TAtualizarDto> : ControllerBase
    where TLeituraDto : class, IRespostaComId
{
    private readonly IServicoCrud<TLeituraDto, TCriarDto, TAtualizarDto> _servicoCrud;

    protected ControladorCrudBase(IServicoCrud<TLeituraDto, TCriarDto, TAtualizarDto> servicoCrud)
    {
        _servicoCrud = servicoCrud;
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

        var registros = await _servicoCrud.ListarAsync(consulta);
        return Ok(registros);
    }

    [HttpGet("{id:guid}")]
    public virtual async Task<ActionResult<TLeituraDto>> ObterPorId(Guid id)
    {
        var registro = await _servicoCrud.ObterPorIdAsync(id);
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
            var registroCriado = await _servicoCrud.CriarAsync(dto);
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
            var registroAtualizado = await _servicoCrud.AtualizarAsync(id, dto);
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
        var removido = await _servicoCrud.RemoverAsync(id);
        if (!removido)
        {
            return NotFound("Registro nao encontrado.");
        }

        return NoContent();
    }
}
