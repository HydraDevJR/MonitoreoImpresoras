using MonitorIMP.Domain.Enums;
using MonitorIMP.Domain.Common;


namespace MonitorIMP.Domain.Entities;

public class UsuarioAcceso : BaseEntity<Guid>
{
    public int UsuarioId { get; set; }

    public NivelAcceso Nivel { get; set; }

    public int? OrganizacionId { get; set; }

    public int? FranquiciaId { get; set; }

    public int? RestauranteId { get; set; }

    public Usuario? Usuario { get; set; }

    public Organizacion? Organizacion { get; set; }

    public Franquicia? Franquicia { get; set; }

    public Restaurante? Restaurante { get; set; }
}