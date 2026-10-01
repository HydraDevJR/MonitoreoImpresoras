namespace MonitorIMP.Domain.Entities;

public class Organizacion : BaseEntity<int>
{
    public string Nombre { get; set; } = string.Empty;
    public string Nit { get; set; } = string.Empty;

    public ICollection<Franquicia> Franquicias { get; set; } = new List<Franquicia>();

    public ICollection<UsuarioAcceso> AccesosUsuario { get; set; } = new List<UsuarioAcceso>();

    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
}