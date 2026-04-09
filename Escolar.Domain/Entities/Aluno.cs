using Escolar.Domain.Models;

namespace Escolar.Domain.Entities;

public class Aluno : ModeloBase
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Email { get; set; }
}
