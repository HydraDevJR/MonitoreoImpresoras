namespace MonitorIMP.Domain.Entities;

public class Restaurante : BaseEntity<int>
{
    public int FranquiciaId { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;

    public Franquicia? Franquicia { get; set; }

    public ICollection<Agente> Agentes { get; set; } = new List<Agente>();
    public ICollection<Impresora> Impresoras { get; set; } = new List<Impresora>();
    public ICollection<UsuarioAcceso> UsuarioAccesos { get; set; } = new List<UsuarioAcceso>();
    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
}