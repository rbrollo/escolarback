using Escolar.Domain.Entities;

namespace Escolar.Domain.Repositories;

public interface IAlunoRepositorio : IRepositorioCrud<Aluno>
{
    Task<(List<Aluno> Itens, int TotalItens)> ListarAsync(int pagina, int tamanhoPagina, string? nome);
    Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null);
}
