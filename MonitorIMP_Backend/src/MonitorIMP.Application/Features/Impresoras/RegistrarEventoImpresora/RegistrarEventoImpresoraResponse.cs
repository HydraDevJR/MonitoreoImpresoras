namespace MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;

public sealed record RegistrarEventoImpresoraResponse(
    Guid EventoId,
    Guid Id,
    bool EsDuplicado,
    DateTime FechaRegistro);