using Escolar.Application.Dtos.Aluno;
using Escolar.Application.Dtos.Shared;
using Escolar.Application.Exceptions;
using Escolar.Application.Services.Shared;
using Escolar.Domain.Entities.Aluno;
using Escolar.Domain.Repositories.Aluno;

namespace Escolar.Application.Services.Aluno;

public class ServiceAluno : ServiceCrudBase<AlunoEntity, AlunoRespostaDto, AlunoCriarDto, AlunoAtualizarDto>, IServiceAluno
{
    private readonly IAlunoRepository _alunoRepository;

    public ServiceAluno(IAlunoRepository alunoRepository)
        : base(alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public override async Task<AlunoRespostaDto> CriarAsync(AlunoCriarDto dto)
    {
        await ValidarCpfDuplicadoAsync(dto.Cpf);
        return await base.CriarAsync(dto);
    }

    public override async Task<AlunoRespostaDto?> AtualizarAsync(Guid id, AlunoAtualizarDto dto)
    {
        await ValidarCpfDuplicadoAsync(dto.Cpf, id);
        return await base.AtualizarAsync(id, dto);
    }

    public async Task<RespostaPaginadaDto<AlunoRespostaDto>> ListarAsync(AlunoConsultaDto consulta)
    {
        var pagina = consulta.ObterPagina();
        var tamanhoPagina = consulta.ObterTamanhoPagina();
        var paginaNormalizada = pagina < 1 ? 1 : pagina;
        var tamanhoPaginaNormalizado = tamanhoPagina < 1 ? 10 : tamanhoPagina;
        var (itens, totalItens) = await _alunoRepository.ListarAsync(
            paginaNormalizada,
            tamanhoPaginaNormalizado,
            consulta.Nome);

        return RespostaPaginadaDto<AlunoRespostaDto>.Criar(
            itens,
            paginaNormalizada,
            tamanhoPaginaNormalizado,
            totalItens,
            MapearParaResposta);
    }

    protected override AlunoEntity CriarEntidade(AlunoCriarDto alunoCriarDto)
    {
        return new AlunoEntity
        {
            Nome = alunoCriarDto.Nome,
            Cpf = alunoCriarDto.Cpf,
            DataNascimento = alunoCriarDto.DataNascimento,
            Email = alunoCriarDto.Email
        };
    }

    protected override void AtualizarEntidade(AlunoEntity alunoExistente, AlunoAtualizarDto alunoAtualizarDto)
    {
        alunoExistente.Nome = alunoAtualizarDto.Nome;
        alunoExistente.Cpf = alunoAtualizarDto.Cpf;
        alunoExistente.DataNascimento = alunoAtualizarDto.DataNascimento;
        alunoExistente.Email = alunoAtualizarDto.Email;
    }

    protected override AlunoRespostaDto MapearParaResposta(AlunoEntity aluno)
    {
        return new AlunoRespostaDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Cpf = aluno.Cpf,
            DataNascimento = aluno.DataNascimento,
            Email = aluno.Email,
            Status = aluno.Status,
            CreatedAt = aluno.CreatedAt,
            UpdatedAt = aluno.UpdatedAt
        };
    }

    private async Task ValidarCpfDuplicadoAsync(string cpf, Guid? ignorarId = null)
    {
        if (await _alunoRepository.ExisteCpfAsync(cpf, ignorarId))
        {
            throw new ValidacaoException("Ja existe um aluno cadastrado com este CPF.");
        }
    }
}
