using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class AgenteConfiguration : IEntityTypeConfiguration<Agente>
{
    public void Configure(EntityTypeBuilder<Agente> builder)
    {
        builder.ToTable("Agentes");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Codigo)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.NombreEquipo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Version)
            .HasMaxLength(30);

        builder.Property(a => a.Estado)
            .IsRequired();

        builder.Property(a => a.UltimaIp)
            .HasMaxLength(45);

        builder.HasIndex(a => new { a.RestauranteId, a.Codigo })
            .IsUnique();

        builder.HasMany(a => a.Impresoras)
            .WithOne(i => i.Agente)
            .HasForeignKey(i => i.AgenteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}