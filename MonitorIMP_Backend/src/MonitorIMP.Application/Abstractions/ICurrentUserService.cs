namespace MonitorIMP.Application.Abstractions;

public interface ICurrentUserService
{
    /// <summary>Id del usuario humano autenticado (null si es agente).</summary>
    int? UsuarioId { get; }

    /// <summary>Id del agente autenticado (null si es usuario).</summary>
    Guid? AgenteId { get; }

    /// <summary>IP de origen del request.</summary>
    string? Ip { get; }

    bool EsUsuario => UsuarioId.HasValue;
    bool EsAgente => AgenteId.HasValue;
    bool EstaAutenticado => EsUsuario || EsAgente;
}