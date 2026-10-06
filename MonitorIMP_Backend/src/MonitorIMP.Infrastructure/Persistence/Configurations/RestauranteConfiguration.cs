using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class RestauranteConfiguration : IEntityTypeConfiguration<Restaurante>
{
    public void Configure(EntityTypeBuilder<Restaurante> builder)
    {
        builder.ToTable("Restaurantes");

        builder.HasKey(r => r.Id);
        // int => identity por defecto

        // FK
        builder.Property(r => r.FranquiciaId)
            .IsRequired();

        builder.Property(r => r.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        // Código único por Franquicia (regla jerárquica)
        builder.HasIndex(r => new { r.FranquiciaId, r.Codigo })
            .IsUnique();

        builder.Property(r => r.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        // Índice para búsquedas por nombre
        builder.HasIndex(r => r.Nombre);

        builder.Property(r => r.Direccion)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Ciudad)
            .IsRequired()
            .HasMaxLength(100);

        // Índice por ciudad
        builder.HasIndex(r => r.Ciudad);

        // ✅ Relación con Franquicia (lado dependiente)
        builder.HasOne(r => r.Franquicia)
            .WithMany(f => f.Restaurantes)
            .HasForeignKey(r => r.FranquiciaId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}