using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Services;
using ProyectoGalacticoWebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Falta la cadena de conexion DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 36))));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalogo Galactico API",
        Version = "v1",
        Description = "API REST para gestionar personajes, cards, eventos, misiones y credenciales del universo Star Wars."
    });
});

builder.Services.AddScoped<IPersonajeService, PersonajeService>();
builder.Services.AddScoped<ICardsPersonajeService, CardsPersonajeService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IMisionService, MisionService>();
builder.Services.AddScoped<IPasswordHasher<CredencialPersonaje>, PasswordHasher<CredencialPersonaje>>();
builder.Services.AddScoped<ICredencialService, CredencialService>();

builder.Services.AddSingleton<ICifradoService, CifradoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();