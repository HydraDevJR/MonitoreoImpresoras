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

        builder.Property(e => e.TipoEvento)
            .IsRequired();

        builder.Property(e => e.FechaEvento)
            .IsRequired();

        builder.Property(e => e.EstadoAnterior);

        builder.Property(e => e.EstadoNuevo)
            .IsRequired();

        builder.Property(e => e.Descripcion)
            .HasMaxLength(500);

        builder.Property(e => e.EventoId);

        builder.HasIndex(e => e.EventoId)
            .IsUnique();

        builder.HasIndex(e => new { e.ImpresoraId, e.FechaEvento });
    }
}