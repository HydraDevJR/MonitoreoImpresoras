using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class UsuarioAccesoConfiguration : IEntityTypeConfiguration<UsuarioAcceso>
{
    public void Configure(EntityTypeBuilder<UsuarioAcceso> builder)
    {
        builder.ToTable("UsuariosAccesos", table =>
        {
            // Nivel: Organizacion = 1, Franquicia = 2, Restaurante = 3
            // Si el enum NivelAcceso cambia, este constraint debe actualizarse.
            table.HasCheckConstraint(
                "CK_UsuariosAccesos_Nivel_Referencia",
                """
                (
                    (Nivel = 1 AND OrganizacionId IS NOT NULL AND FranquiciaId IS NULL AND RestauranteId IS NULL)
                    OR
                    (Nivel = 2 AND OrganizacionId IS NULL AND FranquiciaId IS NOT NULL AND RestauranteId IS NULL)
                    OR
                    (Nivel = 3 AND OrganizacionId IS NULL AND FranquiciaId IS NULL AND RestauranteId IS NOT NULL)
                )
                """);
        });

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        // Enums como int
        builder.Property(a => a.Nivel)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.UsuarioId)
            .IsRequired();

        // FKs nullable
        builder.Property(a => a.OrganizacionId);
        builder.Property(a => a.FranquiciaId);
        builder.Property(a => a.RestauranteId);

        // Relaciones (todas Restrict por soft delete)
        builder.HasOne(a => a.Usuario)
            .WithMany(u => u.UsuarioAccesos)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Organizacion)
            .WithMany(o => o.UsuarioAccesos)
            .HasForeignKey(a => a.OrganizacionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Franquicia)
            .WithMany(f => f.UsuarioAccesos)
            .HasForeignKey(a => a.FranquiciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Restaurante)
            .WithMany(r => r.UsuarioAccesos)
            .HasForeignKey(a => a.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice para accesos activos por usuario (consulta frecuente)
        builder.HasIndex(a => new { a.UsuarioId, a.Activo });

        // Índices filtrados por FK (consultas "quién tiene acceso a X")
        builder.HasIndex(a => a.OrganizacionId)
            .HasFilter("[OrganizacionId] IS NOT NULL");

        builder.HasIndex(a => a.FranquiciaId)
            .HasFilter("[FranquiciaId] IS NOT NULL");

        builder.HasIndex(a => a.RestauranteId)
            .HasFilter("[RestauranteId] IS NOT NULL");

        // ✅ Índices únicos SIN filtro de Activo
        // Garantiza un solo registro por (usuario, scope), activo o no.
        // Reactivar = UPDATE del registro existente, no INSERT.
        builder.HasIndex(a => new { a.UsuarioId, a.OrganizacionId })
            .IsUnique()
            .HasFilter("[OrganizacionId] IS NOT NULL");

        builder.HasIndex(a => new { a.UsuarioId, a.FranquiciaId })
            .IsUnique()
            .HasFilter("[FranquiciaId] IS NOT NULL");

        builder.HasIndex(a => new { a.UsuarioId, a.RestauranteId })
            .IsUnique()
            .HasFilter("[RestauranteId] IS NOT NULL");
    }
}