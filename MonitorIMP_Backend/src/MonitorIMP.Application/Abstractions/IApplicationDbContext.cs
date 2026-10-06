using MonitorIMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MonitorIMP.Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Organizacion> Organizaciones { get; }
    DbSet<Franquicia> Franquicias { get; }
    DbSet<Restaurante> Restaurantes { get; }

    DbSet<Usuario> Usuarios { get; }
    DbSet<UsuarioAcceso> UsuarioAccesos { get; }

    DbSet<Agente> Agentes { get; }
    DbSet<AgenteCredencial> AgenteCredenciales { get; }

    DbSet<Impresora> Impresoras { get; }
    DbSet<ImpresoraEvento> ImpresoraEventos { get; }

    DbSet<Auditoria> Auditorias { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}