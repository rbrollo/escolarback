using Escolar.Domain.Entities;
using Escolar.Domain.Enums;
using Escolar.Domain.Repositories;
using Escolar.Domain.Utils;
using Escolar.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Escolar.Infrastructure.Repositories;

public class AlunoRepositorio : IAlunoRepositorio
{
    private readonly EscolarDbContext _contexto;

    public AlunoRepositorio(EscolarDbContext contexto)
    {
        _contexto = contexto;
    }

    public Task<(List<Aluno> Itens, int TotalItens)> ListarAsync(int pagina, int tamanhoPagina)
    {
        return ListarAsync(pagina, tamanhoPagina, null);
    }

    public async Task<(List<Aluno> Itens, int TotalItens)> ListarAsync(int pagina, int tamanhoPagina, string? nome)
    {
        var query = _contexto.Alunos
            .AsNoTracking()
            .Where(x => x.Status == StatusRegistroEnum.Ativo);

        if (!string.IsNullOrWhiteSpace(nome))
        {
            var nomeFiltro = nome.Trim();
            query = query.Where(x => x.Nome.Contains(nomeFiltro));
        }

        var totalItens = await query.CountAsync();
        var itens = await query
            .OrderBy(x => x.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (itens, totalItens);
    }

    public async Task<Aluno?> ObterPorIdAsync(Guid id)
    {
        return await _contexto.Alunos
            .FirstOrDefaultAsync(x => x.Id == id && x.Status == StatusRegistroEnum.Ativo);
    }

    public async Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null)
    {
        return await _contexto.Alunos
            .AsNoTracking()
            .AnyAsync(x => x.Cpf == cpf && (!ignorarId.HasValue || x.Id != ignorarId.Value));
    }

    public async Task<Aluno> AdicionarAsync(Aluno aluno)
    {
        aluno.CreatedAt = HorarioBrasil.Agora;
        aluno.UpdatedAt = null;
        aluno.DeletedAt = null;
        aluno.Status = StatusRegistroEnum.Ativo;

        _contexto.Alunos.Add(aluno);
        await _contexto.SaveChangesAsync();

        return aluno;
    }

    public async Task<Aluno?> AtualizarAsync(Aluno aluno)
    {
        var alunoExistente = await _contexto.Alunos.FirstOrDefaultAsync(x => x.Id == aluno.Id && x.Status == StatusRegistroEnum.Ativo);
        if (alunoExistente is null)
        {
            return null;
        }

        alunoExistente.Nome = aluno.Nome;
        alunoExistente.Cpf = aluno.Cpf;
        alunoExistente.DataNascimento = aluno.DataNascimento;
        alunoExistente.Email = aluno.Email;
        alunoExistente.Status = aluno.Status;
        alunoExistente.UpdatedAt = HorarioBrasil.Agora;

        await _contexto.SaveChangesAsync();
        return alunoExistente;
    }

    public async Task<bool> RemoverAsync(Guid id)
    {
        var aluno = await _contexto.Alunos.FirstOrDefaultAsync(x => x.Id == id && x.Status == StatusRegistroEnum.Ativo);
        if (aluno is null)
        {
            return false;
        }

        aluno.Status = StatusRegistroEnum.Inativo;
        aluno.DeletedAt = HorarioBrasil.Agora;
        aluno.UpdatedAt = HorarioBrasil.Agora;

        await _contexto.SaveChangesAsync();
        return true;
    }
}
