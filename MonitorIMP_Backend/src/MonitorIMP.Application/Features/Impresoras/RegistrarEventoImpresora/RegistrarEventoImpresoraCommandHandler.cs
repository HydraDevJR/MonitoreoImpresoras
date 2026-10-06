using Microsoft.EntityFrameworkCore;
using MonitorIMP.Application.Abstractions;
using MonitorIMP.Application.Common;
using MonitorIMP.Application.Common.Errors;
using MonitorIMP.Application.Common.Exceptions;
using MonitorIMP.Domain.Entities;
using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;

public sealed class RegistrarEventoImpresoraCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IAuditoriaService _auditoria;

    public RegistrarEventoImpresoraCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IAuditoriaService auditoria)
    {
        _context = context;
        _currentUser = currentUser;
        _auditoria = auditoria;
    }

    public async Task<Result<RegistrarEventoImpresoraResponse>> Handle(
        RegistrarEventoImpresoraCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validar autenticación del agente
        if (!_currentUser.EsAgente || _currentUser.AgenteId is null)
            return Result.Failure<RegistrarEventoImpresoraResponse>(
                CommonErrors.NoAutenticado);

        var agenteId = _currentUser.AgenteId.Value;

        // 2. Verificar duplicado temprano (optimista)
        var existente = await _context.ImpresoraEventos
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.ImpresoraId == request.ImpresoraId
                  && e.EventoId == request.EventoId,
                cancellationToken);

        if (existente is not null)
            return Result.Success(MapToResponse(existente, esDuplicado: true));

        // 3. Cargar impresora activa
        var impresora = await _context.Impresoras
            .FirstOrDefaultAsync(
                i => i.Id == request.ImpresoraId && i.Activo,
                cancellationToken);

        if (impresora is null)
            return Result.Failure<RegistrarEventoImpresoraResponse>(
                RegistrarEventoImpresoraErrors.ImpresoraNotFound);

        // 4. Cargar agente activo
        var agente = await _context.Agentes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.Id == agenteId && a.Activo,
                cancellationToken);

        if (agente is null)
            return Result.Failure<RegistrarEventoImpresoraResponse>(
                CommonErrors.NoAutenticado);

        // 5. Validar pertenencia: agente solo reporta impresoras de su restaurante
        if (impresora.RestauranteId != agente.RestauranteId)
            return Result.Failure<RegistrarEventoImpresoraResponse>(
                RegistrarEventoImpresoraErrors.AgenteNoAutorizado);

        // 6. Construir el evento (EstadoAnterior desde BD, no del payload)
        var estadoAnterior = impresora.Estado;

        var evento = new ImpresoraEvento
        {
            Id = Guid.NewGuid(),
            ImpresoraId = impresora.Id,
            TipoEvento = request.TipoEvento,
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = request.EstadoNuevo ?? estadoAnterior,
            Descripcion = request.Descripcion,
            EventoId = request.EventoId
        };

        _context.ImpresoraEventos.Add(evento);

        // 7. Actualizar estado de la impresora solo si cambia
        if (request.EstadoNuevo.HasValue && request.EstadoNuevo.Value != estadoAnterior)
            impresora.Estado = request.EstadoNuevo.Value;

        // 8. Auditar
        await _auditoria.RegistrarAsync(
            origen: OrigenAuditoria.Agente,
            accion: "RegistrarEventoImpresora",
            entidad: nameof(ImpresoraEvento),
            entidadId: evento.Id.ToString(),
            datosNuevos: new
            {
                evento.EventoId,
                evento.ImpresoraId,
                TipoEvento = evento.TipoEvento.ToString(),
                EstadoAnterior = estadoAnterior.ToString(),
                EstadoNuevo = evento.EstadoNuevo.ToString(),
                evento.Descripcion
            },
            organizacionId: null,
            franquiciaId: null,
            restauranteId: impresora.RestauranteId,
            cancellationToken: cancellationToken);

        // 9. Persistir todo atómicamente
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (UniqueConstraintViolationException ex)
            when (ex.ConstraintName == "IX_ImpresoraEventos_ImpresoraId_EventoId")
        {
            // Alguien insertó el mismo (ImpresoraId, EventoId) en paralelo.
            // Recuperar el evento existente.
            _context.ImpresoraEventos.Remove(evento);

            var eventoExistente = await _context.ImpresoraEventos
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    e => e.ImpresoraId == request.ImpresoraId
                      && e.EventoId == request.EventoId,
                    cancellationToken);

            if (eventoExistente is null)
                return Result.Failure<RegistrarEventoImpresoraResponse>(
                    RegistrarEventoImpresoraErrors.DuplicadoEnConflicto);

            return Result.Success(MapToResponse(eventoExistente, esDuplicado: true));
        }

        // 10. Respuesta exitosa
        return Result.Success(MapToResponse(evento, esDuplicado: false));
    }

    private static RegistrarEventoImpresoraResponse MapToResponse(
        ImpresoraEvento evento,
        bool esDuplicado) =>
        new(
            EventoId: evento.EventoId,
            Id: evento.Id,
            EsDuplicado: esDuplicado,
            FechaRegistro: evento.FechaEvento);
}