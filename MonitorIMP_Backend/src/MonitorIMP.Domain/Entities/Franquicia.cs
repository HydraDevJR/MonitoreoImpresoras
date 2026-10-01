namespace MonitorIMP.Domain.Entities;

public class Franquicia : BaseEntity<int>
{
    public int OrganizacionId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;

    public Organizacion? Organizacion { get; set; }

    public ICollection<Restaurante> Restaurantes { get; set; } = new List<Restaurante>();
    public ICollection<UsuarioAcceso> UsuarioAccesos { get; set; } = new List<UsuarioAcceso>();
    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
}