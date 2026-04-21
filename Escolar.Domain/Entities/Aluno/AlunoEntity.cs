using Escolar.Domain.Models.Shared;

namespace Escolar.Domain.Entities.Aluno;

public class AlunoEntity : ModelBase
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Email { get; set; }
}
