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
        Description = """
        # Catalogo Galactico

        API REST desarrollada con ASP.NET Core Web API para administrar
        personajes, cards, eventos y misiones del universo Star Wars.

        ## Funcionalidades principales

        - **Personajes:** registro, consulta, actualizacion y gestion de estados.
        - **Cards:** administracion de poderes, habilidades y equipamiento.
        - **Eventos:** gestion de participantes, enfrentamientos y resultados.
        - **Misiones:** creacion, asignacion de participantes y ejecucion de misiones.
        - **Credenciales:** almacenamiento seguro mediante hash y cifrado de datos.

        ## Almacenamiento y seguridad

        - Persistencia de datos mediante MySQL y Entity Framework Core.
        - Contraseñas y codigos de acceso protegidos mediante hash.
        - Informacion sensible protegida mediante cifrado AES-GCM.
        - Validaciones y respuestas HTTP para las operaciones de la API.

        ## Documentacion

        Selecciona cualquiera de las operaciones disponibles para consultar
        sus parametros, requisitos, descripciones y respuestas HTTP.
        """
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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalogo Galactico API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();