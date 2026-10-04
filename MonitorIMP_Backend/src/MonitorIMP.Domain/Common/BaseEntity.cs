namespace MonitorIMP.Domain.Common;

public abstract class BaseEntity<TId> : IAuditable
{
    public TId Id { get; set; } = default!;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
}