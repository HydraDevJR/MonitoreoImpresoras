using Microsoft.EntityFrameworkCore;
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
    public DbSet<Restaurante> Restaurantes => Set<Restaurante>();
    public DbSet<Agente> Agentes => Set<Agente>();
    public DbSet<Impresora> Impresoras => Set<Impresora>();
    public DbSet<ImpresoraEvento> ImpresoraEventos => Set<ImpresoraEvento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las configuraciones Fluent API (IEntityTypeConfiguration)
        // que estén definidas en este ensamblado (carpeta Persistence/Configurations)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Interceptor automático para campos de auditoría (FechaCreacion y FechaActualizacion)
        foreach (var entry in ChangeTracker.Entries<BaseEntity<int>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaActualizacion = DateTime.UtcNow;
            }
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity<Guid>>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.FechaCreacion = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.FechaActualizacion = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}