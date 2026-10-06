using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MonitorIMP.Application.Abstractions;

namespace MonitorIMP.Api.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private const string AgenteIdClaim = "agente_id";
    private const string UsuarioIdClaim = "usuario_id";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UsuarioId
    {
        get
        {
            var valor = _httpContextAccessor.HttpContext?.User?
                .FindFirst(UsuarioIdClaim)?.Value;

            return int.TryParse(valor, out var id) ? id : null;
        }
    }

    public Guid? AgenteId
    {
        get
        {
            var valor = _httpContextAccessor.HttpContext?.User?
                .FindFirst(AgenteIdClaim)?.Value;

            return Guid.TryParse(valor, out var id) ? id : null;
        }
    }

    public string? Ip =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
}