using MonitorIMP.Api.Services;
using MonitorIMP.Application;
using MonitorIMP.Application.Abstractions;
using MonitorIMP.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Capas
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Servicios de la capa API
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Controllers / OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ... configuración del pipeline
app.Run();