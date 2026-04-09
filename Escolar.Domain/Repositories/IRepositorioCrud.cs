using Escolar.Domain.Models;

namespace Escolar.Domain.Repositories;

public interface IRepositorioCrud<TEntity>
    where TEntity : ModeloBase
{
    Task<(List<TEntity> Itens, int TotalItens)> ListarAsync(int pagina, int tamanhoPagina);
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<TEntity> AdicionarAsync(TEntity entidade);
    Task<TEntity?> AtualizarAsync(TEntity entidade);
    Task<bool> RemoverAsync(Guid id);
}
