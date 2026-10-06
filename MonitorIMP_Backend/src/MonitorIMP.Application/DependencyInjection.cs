using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MonitorIMP.Application.Features.Impresoras.RegistrarEventoImpresora;

namespace MonitorIMP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // FluentValidation: descubre todos los AbstractValidator<> del ensamblado
        services.AddValidatorsFromAssembly(assembly);

        // Handlers: registro explícito, uno por caso de uso
        services.AddScoped<RegistrarEventoImpresoraCommandHandler>();

        return services;
    }
}