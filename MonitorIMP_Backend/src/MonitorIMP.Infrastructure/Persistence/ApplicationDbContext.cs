using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MonitorIMP.Application.Common.Exceptions;
using MonitorIMP.Domain.Common;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Colecciones DbSet para el Core del sistema
    public DbSet<Organizacion> Organizaciones => Set<Organizacion>();
    public DbSet<Franquicia> Franquicias => Set<Franquicia>();
    public DbSet<Auditoria> Auditorias => Set<Auditoria>();
    public DbSet<Restaurante> Restaurantes => Set<Restaurante>();
    public DbSet<Agente> Agentes => Set<Agente>();
    public DbSet<AgenteCredencial> AgenteCredenciales => Set<AgenteCredencial>();
    public DbSet<Impresora> Impresoras => Set<Impresora>();
    public DbSet<ImpresoraEvento> ImpresoraEventos => Set<ImpresoraEvento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioAcceso> UsuarioAccesos => Set<UsuarioAcceso>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las configuraciones Fluent API (IEntityTypeConfiguration)
        // que estén definidas en este ensamblado (carpeta Persistence/Configurations)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AplicarFechasDeAuditoria();
        return EjecutarConTraduccionAsync(cancellationToken);
    }

    private void AplicarFechasDeAuditoria()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.FechaCreacion = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.FechaActualizacion = DateTime.UtcNow;
                    break;
            }
        }
    }

    private async Task<int> EjecutarConTraduccionAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (EsColisionDeUnicidad(ex))
        {
            throw new UniqueConstraintViolationException(ExtraerNombreConstraint(ex));
        }
    }

    private static bool EsColisionDeUnicidad(DbUpdateException ex)
    {
        return ex.InnerException is SqlException sqlEx
            && sqlEx.Number is 2601 or 2627;
    }

    private static string? ExtraerNombreConstraint(DbUpdateException ex)
    {
        if (ex.InnerException is not SqlException sqlEx)
            return null;

        // SQL Server incluye el nombre del índice/constraint en el mensaje.
        // Formato típico: "Violation of UNIQUE KEY constraint 'IX_...'."
        // o "Cannot insert duplicate key row in object '...' with unique index 'IX_...'."
        var mensaje = sqlEx.Message;

        var match = System.Text.RegularExpressions.Regex.Match(
            mensaje,
            @"(?:constraint|index)\s+'([^']+)'",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return match.Success ? match.Groups[1].Value : null;
    }
}