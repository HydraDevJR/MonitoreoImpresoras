using MonitorIMP.Domain.Entities;
using MonitorIMP.Domain.Enums;

namespace MonitorIMP.Application.Tests.Common;

internal static class TestHelpers
{
    public const int RestauranteIdDefault = 1;
    public const int FranquiciaIdDefault = 1;
    public const int OrganizacionIdDefault = 1;

    public static Agente AgenteActivo(
        Guid? id = null,
        int restauranteId = RestauranteIdDefault,
        string codigo = "AGT-001")
    {
        return new Agente
        {
            Id = id ?? Guid.NewGuid(),
            RestauranteId = restauranteId,
            Codigo = codigo,
            NombreEquipo = "Equipo Test",
            Version = "1.0.0",
            Estado = EstadoAgente.Activo,
            Activo = true
        };
    }

    public static Agente AgenteInactivo(Guid? id = null)
    {
        var agente = AgenteActivo(id);
        agente.Activo = false;
        return agente;
    }

    public static Impresora ImpresoraActiva(
        Guid? id = null,
        Guid? agenteId = null,
        int restauranteId = RestauranteIdDefault,
        EstadoImpresora estado = EstadoImpresora.Online,
        string codigo = "IMP-001")
    {
        return new Impresora
        {
            Id = id ?? Guid.NewGuid(),
            AgenteId = agenteId ?? Guid.NewGuid(),
            RestauranteId = restauranteId,
            Codigo = codigo,
            Nombre = "Impresora Test",
            Estado = estado,
            Activo = true
        };
    }

    public static Impresora ImpresoraInactiva(Guid? id = null)
    {
        var impresora = ImpresoraActiva(id);
        impresora.Activo = false;
        return impresora;
    }
}