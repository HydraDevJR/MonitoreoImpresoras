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

        builder.HasIndex(f => f.Codigo)
            .IsUnique();

        builder.Property(f => f.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasMany(f => f.Restaurantes)
            .WithOne(r => r.Franquicia)
            .HasForeignKey(r => r.FranquiciaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}