namespace MonitorIMP.Domain.Entities;

public class Auditoria : BaseEntity<Guid>
{
    public int? UsuarioId { get; set; }

    public Guid? AgenteId { get; set; }

    public string Accion { get; set; } = string.Empty;

    public string Entidad { get; set; } = string.Empty;

    public string EntidadId { get; set; } = string.Empty;

    public string? DatosAnteriores { get; set; }

    public string? DatosNuevos { get; set; }

    public DateTime FechaEvento { get; set; } = DateTime.UtcNow;

    public string? IpOrigen { get; set; }

    public Usuario? Usuario { get; set; }

    public Agente? Agente { get; set; }
}
