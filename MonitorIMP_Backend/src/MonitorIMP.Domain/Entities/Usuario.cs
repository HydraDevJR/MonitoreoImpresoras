using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Domain.Entities;

public class Usuario : BaseEntity<int>
{
    public string ExternalId { get; set; } = string.Empty;  // ✅ Renombrado
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;    // ✅ NUEVO
    public string Email { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }

    public ICollection<UsuarioAcceso> UsuarioAccesos { get; set; } = new List<UsuarioAcceso>();
    public ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
}