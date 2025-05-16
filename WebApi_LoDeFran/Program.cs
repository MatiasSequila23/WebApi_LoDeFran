using Microsoft.EntityFrameworkCore;
using WebApi_LoDeFran.Mapping;
using WebApi_LoDeFran.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFront", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true) // PERMITE CUALQUIER ORIGEN
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

});

builder.Services.AddDbContext<LoDeFranContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Habilitar CORS
app.UseCors("PermitirFront");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoDeFran API v1");
        c.RoutePrefix = "swagger"; // o "" si querés acceder desde la raíz
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

