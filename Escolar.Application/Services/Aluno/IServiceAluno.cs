

using Escolar.Application.Dtos.Aluno;
using Escolar.Application.Dtos.Shared;
using Escolar.Application.Services.Shared;

namespace Escolar.Application.Services.Aluno;

public interface IServiceAluno : IServiceCrud<AlunoRespostaDto, AlunoCriarDto, AlunoAtualizarDto>
{
    Task<RespostaPaginadaDto<AlunoRespostaDto>> ListarAsync(AlunoConsultaDto consulta);
}
