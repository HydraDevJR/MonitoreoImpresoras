using MonitorIMP.Domain.Enums;
using MonitorIMP.Domain.Common;


namespace MonitorIMP.Domain.Entities;

public class Impresora : BaseEntity<Guid>
{
    public Guid AgenteId { get; set; }
    public int RestauranteId { get; set; }        // ✅ NUEVO

    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Serial { get; set; }
    public string? Mac { get; set; }
    public string? IpActual { get; set; }
    public EstadoImpresora Estado { get; set; }

    // Navegaciones
    public Agente? Agente { get; set; }
    public Restaurante? Restaurante { get; set; } // ✅ NUEVO

    public ICollection<ImpresoraEvento> Eventos { get; set; } = new List<ImpresoraEvento>();
}