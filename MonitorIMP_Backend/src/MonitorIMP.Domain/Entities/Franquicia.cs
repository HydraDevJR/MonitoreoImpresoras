namespace MonitorIMP.Domain.Entities;

public class Franquicia : BaseEntity<int>
{
    public int OrganizacionId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    // Propiedades de navegación de EF Core
    public Organizacion? Organizacion { get; set; }
    public ICollection<Restaurante> Restaurantes { get; set; } = new List<Restaurante>();
}