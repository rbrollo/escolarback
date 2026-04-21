using Escolar.Domain.Models.Shared;

namespace Escolar.Domain.Repositories.Shared;

public interface IRepositoryCrud<TEntity>
    where TEntity : ModelBase
{
    Task<(List<TEntity> Itens, int TotalItens)> ListarAsync(int pagina, int tamanhoPagina);
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<TEntity> AdicionarAsync(TEntity entidade);
    Task<TEntity?> AtualizarAsync(TEntity entidade);
    Task<bool> RemoverAsync(Guid id);
}
