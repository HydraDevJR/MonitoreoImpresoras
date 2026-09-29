namespace MonitorIMP.Domain.Entities;

public class Agente : BaseEntity<Guid>
{
    public int RestauranteId { get; set; }
    public string NombreEquipo { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime? UltimoHeartBeat { get; set; }
    public string? UltimaIp { get; set; }

    // Propiedades de navegación de EF Core
    public Restaurante? Restaurante { get; set; }
    public ICollection<Impresora> Impresoras { get; set; } = new List<Impresora>();
}