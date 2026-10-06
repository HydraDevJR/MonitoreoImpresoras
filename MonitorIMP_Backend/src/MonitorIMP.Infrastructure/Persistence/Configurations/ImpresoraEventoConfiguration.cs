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

        builder.Property(e => e.TipoEvento)
            .IsRequired()
            .HasConversion<int>();

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

        builder.Property(e => e.EventoId).IsRequired();

        // Idempotencia
        builder.HasIndex(e => new { e.ImpresoraId, e.EventoId }).IsUnique();

        // Índices timeline
        builder.HasIndex(e => new { e.ImpresoraId, e.FechaEvento })
            .IsDescending(false, true);

        builder.HasIndex(e => new { e.TipoEvento, e.FechaEvento })
            .IsDescending(false, true);

        // ✅ Timeline general descendente
        builder.HasIndex(e => e.FechaEvento).IsDescending(true);

        // Relación con Impresora
        builder.HasOne(e => e.Impresora)
            .WithMany(i => i.Eventos)
            .HasForeignKey(e => e.ImpresoraId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}