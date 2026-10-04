using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Application.Abstractions;

public interface IAuditoriaService
{
    /// <summary>
    /// Registra una auditoría. NO llama SaveChanges; el handler decide cuándo persistir.
    /// </summary>
    Task RegistrarAsync(
        OrigenAuditoria origen,
        string accion,
        string entidad,
        string entidadId,
        object? datosAnteriores = null,
        object? datosNuevos = null,
        int? organizacionId = null,
        int? franquiciaId = null,
        int? restauranteId = null,
        CancellationToken cancellationToken = default);
}