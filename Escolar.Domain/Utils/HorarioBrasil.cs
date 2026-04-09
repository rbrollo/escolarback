namespace Escolar.Domain.Utils;

public static class HorarioBrasil
{
    public static DateTime Agora => DateTime.UtcNow.AddHours(-3);
}
