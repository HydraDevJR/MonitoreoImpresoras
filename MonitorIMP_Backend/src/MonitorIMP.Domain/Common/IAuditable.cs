namespace MonitorIMP.Domain.Common;

public interface IAuditable
{
    DateTime FechaCreacion { get; set; }
    DateTime? FechaActualizacion { get; set; }
}