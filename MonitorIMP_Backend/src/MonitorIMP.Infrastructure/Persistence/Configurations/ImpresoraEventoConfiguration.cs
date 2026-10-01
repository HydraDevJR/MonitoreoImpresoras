using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class ImpresoraEventoConfiguration : IEntityTypeConfiguration<ImpresoraEvento>
{
    public void Configure(EntityTypeBuilder<ImpresoraEvento> builder)
    {
        builder.ToTable("ImpresoraEventos");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.ImpresoraId).IsRequired();

        // Enum como int (consistente con el proyecto)
        builder.Property(e => e.TipoEvento)
            .IsRequired()
            .HasConversion<int>();

        // Estados como enum
        builder.Property(e => e.EstadoAnterior)
            .HasConversion<int>();

        builder.Property(e => e.EstadoNuevo)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.FechaEvento)
            .IsRequired()
            .HasColumnType("datetime2")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.Descripcion)
            .HasMaxLength(500);

        builder.Property(e => e.EventoId)
            .IsRequired();

        // ✅ Idempotencia: único por impresora
        builder.HasIndex(e => new { e.ImpresoraId, e.EventoId })
            .IsUnique();

        // Índices para consultas frecuentes
        builder.HasIndex(e => new { e.ImpresoraId, e.FechaEvento })
            .IsDescending(false, true);

        builder.HasIndex(e => new { e.TipoEvento, e.FechaEvento })
            .IsDescending(false, true);

        builder.HasIndex(e => e.FechaEvento).IsDescending();

        // Relación con Impresora (historial => Restrict)
        builder.HasOne(e => e.Impresora)
            .WithMany(i => i.Eventos)
            .HasForeignKey(e => e.ImpresoraId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}