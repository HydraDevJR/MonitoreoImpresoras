using MonitorIMP.Domain.Enums;
using MonitorIMP.Domain.Common;


namespace MonitorIMP.Domain.Entities;

public class ImpresoraEvento : BaseEntity<Guid>
{
    public Guid ImpresoraId { get; set; }

    public TipoImpresoraEvento TipoEvento { get; set; }

    public EstadoImpresora? EstadoAnterior { get; set; }
    public EstadoImpresora EstadoNuevo { get; set; }

    public DateTime FechaEvento { get; set; }

    public string? Descripcion { get; set; }

    /// <summary>
    /// Identificador del evento generado por el agente.
    /// Debe ser único por impresora para garantizar idempotencia en reintentos.
    /// El agente debe generarlo UNA VEZ y conservarlo durante todos los reintentos.
    /// </summary>
    public Guid EventoId { get; set; }

    public Impresora? Impresora { get; set; }
}