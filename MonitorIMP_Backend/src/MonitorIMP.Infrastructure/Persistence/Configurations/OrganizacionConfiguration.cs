using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class OrganizacionConfiguration : IEntityTypeConfiguration<Organizacion>
{
    public void Configure(EntityTypeBuilder<Organizacion> builder)
    {
        builder.ToTable("Organizaciones");

        builder.HasKey(o => o.Id);
        // int => identity por defecto

        builder.Property(o => o.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        // ✅ Código único global (es el nivel raíz)
        builder.HasIndex(o => o.Codigo).IsUnique();

        builder.Property(o => o.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        // ✅ Índice para búsquedas por nombre
        builder.HasIndex(o => o.Nombre);

        builder.Property(o => o.Nit)
            .IsRequired()
            .HasMaxLength(20);

        // ✅ NIT único global
        builder.HasIndex(o => o.Nit).IsUnique();
        
    }
}