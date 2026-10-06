namespace MonitorIMP.Domain.Entities;
using MonitorIMP.Domain.Common;


public class AgenteCredencial : BaseEntity<Guid>
{
    public Guid AgenteId { get; set; }

    public string HashSecreto { get; set; } = string.Empty;

    public DateTime FechaExpiracion { get; set; }

    public DateTime? FechaRevocacion { get; set; }

    public DateTime? UltimoUso { get; set; }

    public Agente? Agente { get; set; }
}