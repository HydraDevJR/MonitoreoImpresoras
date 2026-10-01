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
            // Los valores corresponden a:
            // Organizacion = 1
            // Franquicia = 2
            // Restaurante = 3
            //
            // Si se modifica el enum NivelAcceso,
            // esta restricción debe actualizarse.
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

        builder.Property(a => a.Nivel)
            .IsRequired();

        builder.Property(a => a.UsuarioId)
            .IsRequired();

        builder.Property(a => a.OrganizacionId)
            .IsRequired(false);

        builder.Property(a => a.FranquiciaId)
            .IsRequired(false);

        builder.Property(a => a.RestauranteId)
            .IsRequired(false);

        // Relaciones
        builder.HasOne(a => a.Usuario)
            .WithMany(u => u.Accesos)
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Organizacion)
            .WithMany(o => o.AccesosUsuario)
            .HasForeignKey(a => a.OrganizacionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Franquicia)
            .WithMany(f => f.AccesosUsuario)
            .HasForeignKey(a => a.FranquiciaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Restaurante)
            .WithMany(r => r.AccesosUsuario)
            .HasForeignKey(a => a.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices para resolución de alcance
        builder.HasIndex(a => new
        {
            a.UsuarioId,
            a.Activo
        });

        builder.HasIndex(a => a.OrganizacionId);

        builder.HasIndex(a => a.FranquiciaId);

        builder.HasIndex(a => a.RestauranteId);

        // Índices únicos para evitar accesos activos duplicados
        builder.HasIndex(a => new
        {
            a.UsuarioId,
            a.OrganizacionId
        })
        .IsUnique()
        .HasFilter("[Activo] = 1 AND [OrganizacionId] IS NOT NULL");

        builder.HasIndex(a => new
        {
            a.UsuarioId,
            a.FranquiciaId
        })
        .IsUnique()
        .HasFilter("[Activo] = 1 AND [FranquiciaId] IS NOT NULL");

        builder.HasIndex(a => new
        {
            a.UsuarioId,
            a.RestauranteId
        })
        .IsUnique()
        .HasFilter("[Activo] = 1 AND [RestauranteId] IS NOT NULL");
    }
}