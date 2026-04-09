using Escolar.Application.Dtos;
using Escolar.Domain.Models;
using Escolar.Domain.Repositories;
using Escolar.Domain.Utils;

namespace Escolar.Application.Services;

public abstract class ServicoCrudBase<TEntity, TLeituraDto, TCriarDto, TAtualizarDto>
    : IServicoCrud<TLeituraDto, TCriarDto, TAtualizarDto>
    where TEntity : ModeloBase
    where TLeituraDto : class, IRespostaComId
{
    private readonly IRepositorioCrud<TEntity> _repositorio;

    protected ServicoCrudBase(IRepositorioCrud<TEntity> repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<RespostaPaginadaDto<TLeituraDto>> ListarAsync(ConsultaPaginadaDto consulta)
    {
        var pagina = consulta.ObterPagina();
        var tamanhoPagina = consulta.ObterTamanhoPagina();
        var paginaNormalizada = pagina < 1 ? 1 : pagina;
        var tamanhoPaginaNormalizado = tamanhoPagina < 1 ? 10 : tamanhoPagina;
        var (itens, totalItens) = await _repositorio.ListarAsync(paginaNormalizada, tamanhoPaginaNormalizado);

        return RespostaPaginadaDto<TLeituraDto>.Criar(
            itens,
            paginaNormalizada,
            tamanhoPaginaNormalizado,
            totalItens,
            MapearParaResposta);
    }

    public async Task<TLeituraDto?> ObterPorIdAsync(Guid id)
    {
        var entidade = await _repositorio.ObterPorIdAsync(id);
        return entidade is null ? null : MapearParaResposta(entidade);
    }

    public virtual async Task<TLeituraDto> CriarAsync(TCriarDto dto)
    {
        var entidade = CriarEntidade(dto);
        var entidadeCriada = await _repositorio.AdicionarAsync(entidade);
        return MapearParaResposta(entidadeCriada);
    }

    public virtual async Task<TLeituraDto?> AtualizarAsync(Guid id, TAtualizarDto dto)
    {
        var entidadeExistente = await _repositorio.ObterPorIdAsync(id);
        if (entidadeExistente is null)
        {
            return null;
        }

        AtualizarEntidade(entidadeExistente, dto);
        entidadeExistente.UpdatedAt = HorarioBrasil.Agora;

        var entidadeAtualizada = await _repositorio.AtualizarAsync(entidadeExistente);
        return entidadeAtualizada is null ? null : MapearParaResposta(entidadeAtualizada);
    }

    public Task<bool> RemoverAsync(Guid id)
    {
        return _repositorio.RemoverAsync(id);
    }

    protected abstract TLeituraDto MapearParaResposta(TEntity entidade);

    protected abstract TEntity CriarEntidade(TCriarDto dto);

    protected abstract void AtualizarEntidade(TEntity entidade, TAtualizarDto dto);
}
