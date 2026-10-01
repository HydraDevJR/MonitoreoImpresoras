using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class CredencialAgenteConfiguration : IEntityTypeConfiguration<AgenteCredencial>
{
    public void Configure(EntityTypeBuilder<AgenteCredencial> builder)
    {
        builder.ToTable("AgenteCredenciales");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.HashSecreto)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.FechaExpiracion)
            .IsRequired();

        builder.Property(c => c.FechaRevocacion)
            .IsRequired(false);

        builder.Property(c => c.UltimoUso)
            .IsRequired(false);

        builder.HasIndex(c => c.AgenteId);

        builder.HasOne(c => c.Agente)
            .WithMany()
            .HasForeignKey(c => c.AgenteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}