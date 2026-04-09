using Escolar.Domain.Enums;

namespace Escolar.Application.Dtos;

public class AlunoRespostaDto : IRespostaComId
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string? Email { get; set; }
    public StatusRegistroEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
