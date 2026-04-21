using Escolar.Application.Dtos.Shared;
using Escolar.Domain.Models.Shared;
using Escolar.Domain.Repositories.Shared;
using Escolar.Domain.Utils;

namespace Escolar.Application.Services.Shared;

public abstract class ServiceCrudBase<TEntity, TLeituraDto, TCriarDto, TAtualizarDto>
    : IServiceCrud<TLeituraDto, TCriarDto, TAtualizarDto>
    where TEntity : ModelBase
    where TLeituraDto : class, IRespostaComId
{
    private readonly IRepositoryCrud<TEntity> _repository;

    protected ServiceCrudBase(IRepositoryCrud<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<RespostaPaginadaDto<TLeituraDto>> ListarAsync(ConsultaPaginadaDto consulta)
    {
        var pagina = consulta.ObterPagina();
        var tamanhoPagina = consulta.ObterTamanhoPagina();
        var paginaNormalizada = pagina < 1 ? 1 : pagina;
        var tamanhoPaginaNormalizado = tamanhoPagina < 1 ? 10 : tamanhoPagina;
        var (itens, totalItens) = await _repository.ListarAsync(paginaNormalizada, tamanhoPaginaNormalizado);

        return RespostaPaginadaDto<TLeituraDto>.Criar(
            itens,
            paginaNormalizada,
            tamanhoPaginaNormalizado,
            totalItens,
            MapearParaResposta);
    }

    public async Task<TLeituraDto?> ObterPorIdAsync(Guid id)
    {
        var entidade = await _repository.ObterPorIdAsync(id);
        return entidade is null ? null : MapearParaResposta(entidade);
    }

    public virtual async Task<TLeituraDto> CriarAsync(TCriarDto dto)
    {
        var entidade = CriarEntidade(dto);
        var entidadeCriada = await _repository.AdicionarAsync(entidade);
        return MapearParaResposta(entidadeCriada);
    }

    public virtual async Task<TLeituraDto?> AtualizarAsync(Guid id, TAtualizarDto dto)
    {
        var entidadeExistente = await _repository.ObterPorIdAsync(id);
        if (entidadeExistente is null)
        {
            return null;
        }

        AtualizarEntidade(entidadeExistente, dto);
        entidadeExistente.UpdatedAt = HorarioBrasil.Agora;

        var entidadeAtualizada = await _repository.AtualizarAsync(entidadeExistente);
        return entidadeAtualizada is null ? null : MapearParaResposta(entidadeAtualizada);
    }

    public Task<bool> RemoverAsync(Guid id)
    {
        return _repository.RemoverAsync(id);
    }

    protected abstract TLeituraDto MapearParaResposta(TEntity entidade);

    protected abstract TEntity CriarEntidade(TCriarDto dto);

    protected abstract void AtualizarEntidade(TEntity entidade, TAtualizarDto dto);
}
