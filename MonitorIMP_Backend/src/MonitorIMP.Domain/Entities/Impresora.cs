namespace MonitorIMP.Domain.Entities;

public class Impresora : BaseEntity<Guid>
{
    public Guid AgenteId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Serial { get; set; }
    public string? Mac { get; set; }
    public string? IpActual { get; set; }
    public string EstadoActual { get; set; } = string.Empty;

    // Propiedades de navegación de EF Core
    public Agente? Agente { get; set; }
    public ICollection<EventosImpresora> Eventos { get; set; } = new List<EventosImpresora>();
}