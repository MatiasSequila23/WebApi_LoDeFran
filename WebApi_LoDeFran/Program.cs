using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Mapping;
using WebApi_LoDeFran.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS (abierto para pruebas en LAN)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFront", policy =>
    {
        policy
            .AllowAnyOrigin()   // <- Orígenes abiertos (rápido para probar)
            .AllowAnyHeader()
            .AllowAnyMethod();
        // Si más adelante necesitás cookies/tokens con credenciales,
        // cambiamos esto por .AllowCredentials() + .WithOrigins(...)
    });
});

builder.Services.AddDbContext<LoDeFranContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// (Opcional) Header para Private Network Access (preflight desde otras PCs en LAN)
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS" &&
        context.Request.Headers.ContainsKey("Access-Control-Request-Private-Network"))
    {
        context.Response.Headers.Append("Access-Control-Allow-Private-Network", "true");
    }
    await next();
});

// Swagger (como lo tenías)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoDeFran API v1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection(); // Seguimos en HTTP en LAN

// Orden recomendado: CORS antes de Authorization y antes de MapControllers
app.UseCors("PermitirFront");
app.UseAuthorization();

app.MapControllers();

app.Run();
