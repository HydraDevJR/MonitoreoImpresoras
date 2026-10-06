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

        builder.Property(f => f.OrganizacionId).IsRequired();

        builder.Property(f => f.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(f => new { f.OrganizacionId, f.Codigo }).IsUnique();

        builder.Property(f => f.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        // ✅ Índice para búsquedas por nombre
        builder.HasIndex(f => f.Nombre);

        // Relación con Organizacion
        builder.HasOne(f => f.Organizacion)
            .WithMany(o => o.Franquicias)
            .HasForeignKey(f => f.OrganizacionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}