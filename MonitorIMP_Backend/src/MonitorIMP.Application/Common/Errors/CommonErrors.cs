
namespace MonitorIMP.Application.Common.Errors;

public static class CommonErrors
{
    public static readonly Error NoAutenticado = Error.Unauthorized(
        "General.NoAutenticado",
        "El usuario no está autenticado.");

    public static readonly Error ErrorInesperado = Error.Unexpected(
        "General.ErrorInesperado",
        "Ocurrió un error inesperado.");
}