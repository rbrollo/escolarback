using Escolar.Application.Dtos.Shared;

namespace Escolar.Application.Services.Shared;

public interface IServiceCrud<TLeituraDto, in TCriarDto, in TAtualizarDto>
    where TLeituraDto : class, IRespostaComId
{
    Task<RespostaPaginadaDto<TLeituraDto>> ListarAsync(ConsultaPaginadaDto consulta);
    Task<TLeituraDto?> ObterPorIdAsync(Guid id);
    Task<TLeituraDto> CriarAsync(TCriarDto dto);
    Task<TLeituraDto?> AtualizarAsync(Guid id, TAtualizarDto dto);
    Task<bool> RemoverAsync(Guid id);
}
