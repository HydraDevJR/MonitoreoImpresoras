using MonitorIMP.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar servicios de Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// 2. Habilitar Swagger UI en entorno de Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MonitorIMP API v1");
        c.RoutePrefix = "swagger"; // La interfaz quedará en /swagger
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();