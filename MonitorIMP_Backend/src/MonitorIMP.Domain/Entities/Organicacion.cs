namespace MonitorIMP.Domain.Entities;

public class Organizacion : BaseEntity<int>
{
    public string Nombre { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;

    // Relación 1 a N con Franquicia
    public ICollection<Franquicia> Franquicias { get; set; } = new List<Franquicia>();
}