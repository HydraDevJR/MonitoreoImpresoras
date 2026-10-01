using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.ToTable("Auditorias", table =>
        {
            // Regla: el origen determina qué actores son válidos.
            // - Usuario  => UsuarioId obligatorio, AgenteId nulo
            // - Agente   => AgenteId obligatorio,  UsuarioId nulo
            // - Sistema/Api/Migracion => ambos nulos
            table.HasCheckConstraint(
                "CK_Auditorias_OrigenActor",
                """
                (
                    (Origen = 1 AND UsuarioId IS NOT NULL AND AgenteId IS NULL)
                    OR
                    (Origen = 2 AND AgenteId IS NOT NULL AND UsuarioId IS NULL)
                    OR
                    (Origen IN (3, 4, 5) AND UsuarioId IS NULL AND AgenteId IS NULL)
                )
                """);
        });

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Origen)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.Accion)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Entidad)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.EntidadId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.DatosAnteriores)
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.DatosNuevos)
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.FechaEvento)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.IpOrigen)
            .HasColumnType("varchar(45)");

        // Índices filtrados por actor
        builder.HasIndex(a => new { a.UsuarioId, a.FechaEvento })
            .HasFilter("[UsuarioId] IS NOT NULL");

        builder.HasIndex(a => new { a.AgenteId, a.FechaEvento })
            .HasFilter("[AgenteId] IS NOT NULL");

        // Índices filtrados por alcance
        builder.HasIndex(a => new { a.OrganizacionId, a.FechaEvento })
            .HasFilter("[OrganizacionId] IS NOT NULL");

        builder.HasIndex(a => new { a.FranquiciaId, a.FechaEvento })
            .HasFilter("[FranquiciaId] IS NOT NULL");

        builder.HasIndex(a => new { a.RestauranteId, a.FechaEvento })
            .HasFilter("[RestauranteId] IS NOT NULL");

        // Índice por origen (para analítica)
        builder.HasIndex(a => new { a.Origen, a.FechaEvento })
            .IsDescending(false, true);

        // Historial de una entidad específica
        builder.HasIndex(a => new { a.Entidad, a.EntidadId, a.FechaEvento })
            .IsDescending(false, false, true);

        // Línea de tiempo general
        builder.HasIndex(a => a.FechaEvento)
            .IsDescending();

        // Relaciones (siempre Restrict en auditoría)
        builder.HasOne(a => a.Usuario)
            .WithMany(u => u.Auditorias)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Agente)
            .WithMany(a => a.Auditorias)
            .HasForeignKey(a => a.AgenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Organizacion)
            .WithMany(o => o.Auditorias)
            .HasForeignKey(a => a.OrganizacionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Franquicia)
            .WithMany(f => f.Auditorias)
            .HasForeignKey(a => a.FranquiciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Restaurante)
            .WithMany(r => r.Auditorias)
            .HasForeignKey(a => a.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}