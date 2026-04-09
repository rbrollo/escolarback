using Escolar.Domain.Enums;
using Escolar.Domain.Utils;

namespace Escolar.Domain.Models;

public abstract class ModeloBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public StatusRegistroEnum Status { get; set; } = StatusRegistroEnum.Ativo;
    public DateTime CreatedAt { get; set; } = HorarioBrasil.Agora;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
