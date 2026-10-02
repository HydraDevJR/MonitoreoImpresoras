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
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.RestauranteId).IsRequired();

        builder.Property(a => a.Codigo)
            .IsRequired()
            .HasMaxLength(30);

        // Código único por restaurante
        builder.HasIndex(a => new { a.RestauranteId, a.Codigo })
            .IsUnique();

        builder.Property(a => a.NombreEquipo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Version)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.Estado)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.UltimoHeartBeat)
            .HasColumnType("datetime2");

        builder.Property(a => a.UltimaIp)
            .HasColumnType("varchar(45)")
            .HasMaxLength(45);

        // Índices para consultas frecuentes
        builder.HasIndex(a => a.Estado);
        builder.HasIndex(a => a.UltimoHeartBeat);
        builder.HasIndex(a => new { a.Estado, a.UltimoHeartBeat });

        // Relación con Restaurante
        builder.HasOne(a => a.Restaurante)
            .WithMany(r => r.Agentes)
            .HasForeignKey(a => a.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}