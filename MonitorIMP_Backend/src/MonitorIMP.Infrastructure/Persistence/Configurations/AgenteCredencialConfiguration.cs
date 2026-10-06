using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class AgenteCredencialConfiguration : IEntityTypeConfiguration<AgenteCredencial>
{
    public void Configure(EntityTypeBuilder<AgenteCredencial> builder)
    {
        builder.ToTable("AgenteCredenciales");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.HashSecreto)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.FechaExpiracion)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(c => c.FechaRevocacion)
            .HasColumnType("datetime2");

        builder.Property(c => c.UltimoUso)
            .HasColumnType("datetime2");

        // ✅ Índice compuesto (reemplaza el simple por AgenteId)
        builder.HasIndex(c => new { c.AgenteId, c.Activo });

        // Relación con Agente
        builder.HasOne(c => c.Agente)
            .WithMany(a => a.Credenciales)
            .HasForeignKey(c => c.AgenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}