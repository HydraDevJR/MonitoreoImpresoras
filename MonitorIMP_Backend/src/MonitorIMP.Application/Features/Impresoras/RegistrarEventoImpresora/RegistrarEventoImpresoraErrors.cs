using MonitorIMP.Application.Common.Errors;

namespace MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;

public static class RegistrarEventoImpresoraErrors
{
    public static readonly Error ImpresoraNotFound = Error.NotFound(
        "Impresora.NotFound",
        "La impresora no existe o está inactiva.");

    public static readonly Error AgenteNoAutorizado = Error.Forbidden(
        "Agente.NoAutorizadoParaImpresora",
        "El agente no está autorizado para reportar eventos de esta impresora.");

    public static readonly Error DuplicadoEnConflicto = Error.Conflict(
        "ImpresoraEvento.DuplicadoEnConflicto",
        "El evento ya existe y no pudo recuperarse tras la colisión.");
}