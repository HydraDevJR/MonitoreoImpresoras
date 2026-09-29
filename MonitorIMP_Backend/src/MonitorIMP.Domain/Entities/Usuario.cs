using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Domain.Entities;

public class Usuario : BaseEntity<int>
{
    public string EntraObjectId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
}