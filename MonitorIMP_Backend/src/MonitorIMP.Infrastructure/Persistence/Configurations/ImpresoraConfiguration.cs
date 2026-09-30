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

        builder.Property(i => i.Codigo)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(i => i.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Serial)
            .HasMaxLength(50);

        builder.Property(i => i.Mac)
            .HasMaxLength(17);

        builder.Property(i => i.IpActual)
            .HasMaxLength(45);

        builder.Property(i => i.Estado)
            .IsRequired();

        builder.HasMany(i => i.Eventos)
            .WithOne(e => e.Impresora)
            .HasForeignKey(e => e.ImpresoraId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}