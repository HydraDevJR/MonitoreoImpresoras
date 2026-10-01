using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class FranquiciaConfiguration : IEntityTypeConfiguration<Franquicia>
{
    public void Configure(EntityTypeBuilder<Franquicia> builder)
    {
        builder.ToTable("Franquicias");

        builder.HasKey(f => f.Id);
        // int => identity por defecto (ValueGeneratedOnAdd)

        // ✅ Único por Organización
        builder.HasIndex(f => new { f.OrganizacionId, f.Codigo })
            .IsUnique();

        builder.Property(f => f.OrganizacionId)
            .IsRequired();

        builder.Property(f => f.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        // ✅ Relación con Organizacion (configurada aquí, lado dependiente)
        builder.HasOne(f => f.Organizacion)
            .WithMany(o => o.Franquicias) // verificar que Organizacion tenga esta colección
            .HasForeignKey(f => f.OrganizacionId)
            .OnDelete(DeleteBehavior.Restrict);

        // ❌ Relación con Restaurantes: se configura en RestauranteConfiguration
        // ❌ Relación con AccesosUsuario: se configura en UsuarioAccesoConfiguration
        // ❌ Relación con Auditorias: se configura en AuditoriaConfiguration
    }
}