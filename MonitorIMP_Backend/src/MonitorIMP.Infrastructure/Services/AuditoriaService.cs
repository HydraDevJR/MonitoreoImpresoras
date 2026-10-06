using System.Text.Json;
using MonitorIMP.Application.Abstractions;
using MonitorIMP.Domain.Entities;
using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Infrastructure.Services;

public sealed class AuditoriaService : IAuditoriaService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AuditoriaService(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public Task RegistrarAsync(
        OrigenAuditoria origen,
        string accion,
        string entidad,
        string entidadId,
        object? datosAnteriores = null,
        object? datosNuevos = null,
        int? organizacionId = null,
        int? franquiciaId = null,
        int? restauranteId = null,
        CancellationToken cancellationToken = default)
    {
        // Actor actual (viene del token, nunca del payload)
        var usuarioId = _currentUser.UsuarioId;
        var agenteId = _currentUser.AgenteId;

        // Validación de coherencia Origen ↔ Actor
        // La restricción de BD (CK_Auditorias_OrigenActor) es la última barrera.
        // Aquí fallamos antes, con un mensaje claro.
        switch (origen)
        {
            case OrigenAuditoria.Usuario:
                if (usuarioId is null)
                    throw new InvalidOperationException(
                        "Origen=Usuario requiere un usuario autenticado.");
                if (agenteId is not null)
                    throw new InvalidOperationException(
                        "Origen=Usuario no admite AgenteId.");
                break;

            case OrigenAuditoria.Agente:
                if (agenteId is null)
                    throw new InvalidOperationException(
                        "Origen=Agente requiere un agente autenticado.");
                if (usuarioId is not null)
                    throw new InvalidOperationException(
                        "Origen=Agente no admite UsuarioId.");
                break;

            case OrigenAuditoria.Sistema:
            case OrigenAuditoria.Api:
            case OrigenAuditoria.Migracion:
                if (usuarioId is not null || agenteId is not null)
                    throw new InvalidOperationException(
                        $"Origen={origen} no admite UsuarioId ni AgenteId.");
                break;

            default:
                throw new InvalidOperationException(
                    $"Origen '{origen}' no es válido.");
        }

        var auditoria = new Auditoria
        {
            Id = Guid.NewGuid(),
            Origen = origen,
            UsuarioId = usuarioId,
            AgenteId = agenteId,
            OrganizacionId = organizacionId,
            FranquiciaId = franquiciaId,
            RestauranteId = restauranteId,
            Accion = accion,
            Entidad = entidad,
            EntidadId = entidadId,
            DatosAnteriores = Serializar(datosAnteriores),
            DatosNuevos = Serializar(datosNuevos),
            // FechaEvento: default GETUTCDATE() en SQL Server.
            IpOrigen = _currentUser.Ip
        };

        _context.Auditorias.Add(auditoria);

        // ⚠️ NO se llama SaveChangesAsync aquí. El handler decide cuándo persistir.

        return Task.CompletedTask;
    }

    private static string? Serializar(object? valor)
    {
        if (valor is null)
            return null;

        return JsonSerializer.Serialize(valor, JsonOptions);
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
}