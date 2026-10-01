using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Domain.Entities;

public class Auditoria : BaseEntity<Guid>
{
    public OrigenAuditoria Origen { get; set; }

    public int? UsuarioId { get; set; }
    public Guid? AgenteId { get; set; }

    public int? OrganizacionId { get; set; }
    public int? FranquiciaId { get; set; }
    public int? RestauranteId { get; set; }

    public string Accion { get; set; } = string.Empty;
    public string Entidad { get; set; } = string.Empty;
    public string EntidadId { get; set; } = string.Empty;

    public string? DatosAnteriores { get; set; }
    public string? DatosNuevos { get; set; }

    public DateTime FechaEvento { get; set; } = DateTime.UtcNow;

    public string? IpOrigen { get; set; }

    public Usuario? Usuario { get; set; }
    public Agente? Agente { get; set; }

    public Organizacion? Organizacion { get; set; }
    public Franquicia? Franquicia { get; set; }
    public Restaurante? Restaurante { get; set; }
}