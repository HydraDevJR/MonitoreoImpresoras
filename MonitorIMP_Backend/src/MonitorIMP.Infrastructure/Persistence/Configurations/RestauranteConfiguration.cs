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

        builder.Property(r => r.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(r => r.Direccion)
            .HasMaxLength(200);

        builder.Property(r => r.Ciudad)
            .HasMaxLength(100);

        builder.HasIndex(r => new { r.FranquiciaId, r.Codigo })
            .IsUnique();

        builder.HasMany(r => r.Agentes)
            .WithOne(a => a.Restaurante)
            .HasForeignKey(a => a.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}