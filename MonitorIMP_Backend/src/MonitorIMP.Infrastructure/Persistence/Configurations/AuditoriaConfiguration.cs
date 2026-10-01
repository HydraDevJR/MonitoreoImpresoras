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
            table.HasCheckConstraint(
                "CK_Auditorias_UnSoloActor",
                """
                (
                    UsuarioId IS NULL
                    OR AgenteId IS NULL
                )
                """);
        });

        builder.HasKey(a => a.Id);

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
            .IsRequired(false);

        builder.Property(a => a.DatosNuevos)
            .IsRequired(false);

        builder.Property(a => a.FechaEvento)
            .IsRequired();

        builder.Property(a => a.IpOrigen)
            .IsRequired(false)
            .HasMaxLength(45);

        // Auditorías realizadas por usuarios
        builder.HasIndex(a => new
        {
            a.UsuarioId,
            a.FechaEvento
        })
        .HasFilter("[UsuarioId] IS NOT NULL");

        // Auditorías generadas por agentes
        builder.HasIndex(a => new
        {
            a.AgenteId,
            a.FechaEvento
        })
        .HasFilter("[AgenteId] IS NOT NULL");

        // Consultas por alcance
        builder.HasIndex(a => new
        {
            a.OrganizacionId,
            a.FechaEvento
        });

        builder.HasIndex(a => new
        {
            a.FranquiciaId,
            a.FechaEvento
        });

        builder.HasIndex(a => new
        {
            a.RestauranteId,
            a.FechaEvento
        });

        // Historial de una entidad específica
        builder.HasIndex(a => new
        {
            a.Entidad,
            a.EntidadId,
            a.FechaEvento
        });

        // Línea de tiempo general
        builder.HasIndex(a => a.FechaEvento);

        // Relaciones
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