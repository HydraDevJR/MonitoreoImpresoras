using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class ImpresoraConfiguration : IEntityTypeConfiguration<Impresora>
{
    public void Configure(EntityTypeBuilder<Impresora> builder)
    {
        builder.ToTable("Impresoras");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property(i => i.AgenteId).IsRequired();
        builder.Property(i => i.RestauranteId).IsRequired();

        builder.Property(i => i.Codigo)
            .IsRequired()
            .HasMaxLength(30);

        // Código único por restaurante
        builder.HasIndex(i => new { i.RestauranteId, i.Codigo }).IsUnique();

        builder.Property(i => i.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Serial)
            .HasMaxLength(50);

        builder.HasIndex(i => i.Serial)
            .IsUnique()
            .HasFilter("[Serial] IS NOT NULL");

        builder.Property(i => i.Mac)
            .HasMaxLength(17);

        builder.HasIndex(i => i.Mac)
            .IsUnique()
            .HasFilter("[Mac] IS NOT NULL");

        // ✅ IP como varchar(45)
        builder.Property(i => i.IpActual)
            .HasColumnType("varchar(45)")
            .HasMaxLength(45);

        builder.Property(i => i.Estado)
            .IsRequired()
            .HasConversion<int>();

        // Relaciones
        builder.HasOne(i => i.Agente)
            .WithMany(a => a.Impresoras)
            .HasForeignKey(i => i.AgenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Restaurante)
            .WithMany(r => r.Impresoras)
            .HasForeignKey(i => i.RestauranteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices
        builder.HasIndex(i => i.AgenteId);
        builder.HasIndex(i => i.RestauranteId);
        builder.HasIndex(i => i.Estado);
    }
}