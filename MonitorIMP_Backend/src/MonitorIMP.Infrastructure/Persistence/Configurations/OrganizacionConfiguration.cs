using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class OrganizacionConfiguration : IEntityTypeConfiguration<Organizacion>
{
    public void Configure(EntityTypeBuilder<Organizacion> builder)
    {
        builder.ToTable("Organizaciones");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(o => o.Nit)
            .IsUnique();

        builder.Property(o => o.Nit)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasMany(o => o.Franquicias)
            .WithOne(f => f.Organizacion)
            .HasForeignKey(f => f.OrganizacionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}