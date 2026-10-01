using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonitorIMP.Domain.Entities;

namespace MonitorIMP.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);
        // int => identity por defecto

        builder.Property(u => u.ExternalId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.ExternalId).IsUnique();

        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Apellido)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150)
            .UseCollation("SQL_Latin1_General_CP1_CI_AS"); // ✅ Opcional, refuerza unicidad case-insensitive

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Rol)
            .IsRequired()
            .HasConversion<int>();

        // Índices para búsquedas
        builder.HasIndex(u => u.Nombre);
        builder.HasIndex(u => u.Apellido);
        builder.HasIndex(u => u.Rol);
    }
}