namespace MonitorIMP.Domain.Entities;

public class Restaurante : BaseEntity<int>
{
    public int FranquiciaId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;

    // Propiedades de navegación de EF Core
    public Franquicia? Franquicia { get; set; }
    public ICollection<Agente> Agentes { get; set; } = new List<Agente>();
}