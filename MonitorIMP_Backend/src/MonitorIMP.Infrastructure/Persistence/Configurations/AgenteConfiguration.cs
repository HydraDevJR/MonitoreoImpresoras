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

        builder.Property(a => a.RestauranteId)
            .IsRequired();

        // ✅ Código único global (consistente con Impresora)
        builder.HasIndex(a => a.Codigo).IsUnique();

        builder.Property(a => a.Codigo)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.NombreEquipo)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Version)
            .IsRequired()
            .HasMaxLength(30);

        // ✅ Enum como int (consistente con el proyecto)
        builder.Property(a => a.Estado)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.UltimoHeartBeat)
            .HasColumnType("datetime2");

        builder.Property(a => a.UltimaIp)
            .HasMaxLength(45);

        // ✅ Relación con Restaurante (configurada en el dependiente)
        builder.HasOne(a => a.Restaurante)
            .WithMany(r => r.Agentes) // verificar que Restaurante tenga la colección
            .HasForeignKey(a => a.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}