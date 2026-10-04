using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;

public sealed record RegistrarEventoImpresoraCommand(
    Guid EventoId,
    Guid ImpresoraId,
    TipoImpresoraEvento TipoEvento,
    EstadoImpresora? EstadoNuevo,
    string? Descripcion);