namespace MonitorIMP.Domain.Entities;

public class EventosImpresora : BaseEntity<Guid>
{
    public Guid ImpresoraId { get; set; }
    public string TipoEvento { get; set; } = string.Empty;
    public string? EstadoAnterior { get; set; }
    public string EstadoNuevo { get; set; } = string.Empty;
    public DateTime FechaEvento { get; set; } = DateTime.UtcNow;
    public string? Descripcion { get; set; }
    public string? EventoId { get; set; }

    // Propiedad de navegación de EF Core
    public Impresora? Impresora { get; set; }
}