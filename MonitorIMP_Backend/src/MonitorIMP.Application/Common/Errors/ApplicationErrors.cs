using MonitorIMP.Application.Common.Errors;

namespace MonitorIMP.Application.Common.Errors;

public static class ApplicationErrors
{
    public static class Impresora
    {
        // No distinguimos entre "no existe" e "inactiva" hacia el exterior,
        // para no revelar existencia a agentes no autorizados.
        public static readonly Error NotFound = Error.NotFound(
            "Impresora.NotFound",
            "La impresora no está disponible.");
    }

    public static class Agente
    {
        public static readonly Error NotFound = Error.NotFound(
            "Agente.NotFound",
            "El agente no existe o está inactivo.");

        public static readonly Error NoAutorizadoParaImpresora = Error.Forbidden(
            "Agente.NoAutorizadoParaImpresora",
            "El agente no está autorizado para reportar eventos de esta impresora.");
    }

    public static class ImpresoraEvento
    {
        public static readonly Error TipoEventoInvalido = Error.Validation(
            "ImpresoraEvento.TipoEventoInvalido",
            "El tipo de evento no es válido.");

        public static readonly Error EstadoNuevoInvalido = Error.Validation(
            "ImpresoraEvento.EstadoNuevoInvalido",
            "El estado nuevo no es válido.");

        // Solo se retorna cuando hubo colisión de unicidad
        // y NO se pudo recuperar el evento existente.
        public static readonly Error DuplicadoEnConflicto = Error.Conflict(
            "ImpresoraEvento.DuplicadoEnConflicto",
            "El evento ya existe y no pudo recuperarse tras la colisión.");
    }

    public static class General
    {
        public static readonly Error NoAutenticado = Error.Unauthorized(
            "General.NoAutenticado",
            "El usuario no está autenticado.");

        public static readonly Error ErrorInesperado = Error.Unexpected(
            "General.ErrorInesperado",
            "Ocurrió un error inesperado.");
    }
}