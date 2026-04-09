using Escolar.Application.Dtos;

namespace Escolar.Application.Services;

public interface IServicoAluno : IServicoCrud<AlunoRespostaDto, AlunoCriarDto, AlunoAtualizarDto>
{
    Task<RespostaPaginadaDto<AlunoRespostaDto>> ListarAsync(AlunoConsultaDto consulta);
}
