using Escolar.Domain.Entities.Aluno;
using Escolar.Domain.Repositories.Shared;

namespace Escolar.Domain.Repositories.Aluno;

public interface IAlunoRepository : IRepositoryCrud<AlunoEntity>
{
    Task<(List<AlunoEntity> Itens, int TotalItens)> ListarAsync(int pagina, int tamanhoPagina, string? nome);
    Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null);
}
