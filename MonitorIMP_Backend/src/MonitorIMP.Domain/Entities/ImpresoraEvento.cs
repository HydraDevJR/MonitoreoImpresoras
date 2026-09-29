using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Domain.Entities;

public class ImpresoraEvento : BaseEntity<Guid>
{
    public Guid ImpresoraId { get; set; }
    public TipoImpresoraEvento TipoEvento { get; set; }
    public string? EstadoAnterior { get; set; }
    public string EstadoNuevo { get; set; } = string.Empty;
    public DateTime FechaEvento { get; set; } = DateTime.UtcNow;
    public string? Descripcion { get; set; }
    public Guid EventoId { get; set; }

    // Propiedad de navegación de EF Core
    public Impresora? Impresora { get; set; }
}