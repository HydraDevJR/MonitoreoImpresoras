using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonitorIMP.Application.Abstractions;
using MonitorIMP.Infrastructure.Persistence;
using MonitorIMP.Infrastructure.Services;

namespace MonitorIMP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión 'DefaultConnection'.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        // Servicios de infraestructura
        services.AddScoped<IAuditoriaService, AuditoriaService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}