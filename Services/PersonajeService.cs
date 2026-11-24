using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProyectoGalacticoWebApi.Services
{
    public class PersonajeService : IPersonajeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PersonajeService> _logger;

        public PersonajeService(AppDbContext context, ILogger<PersonajeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PersonajeDto>> ListarPersonajesAsync()
        {
            var personajes = await _context.Personajes.AsNoTracking().ToListAsync();

            _logger.LogInformation("Se listaron {Cantidad} personajes", personajes.Count);

            return personajes.Select(p => new PersonajeDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Especie = p.Especie,
                Faccion = p.Faccion.ToString(),
                Afiliacion = p.Afiliacion,
                Estado = p.Estado?.ToString(),
                FuerzaSensitiva = p.FuerzaSensitiva,
                ImagenUrl = p.ImagenUrl
            });
        }

        public async Task<PersonajeDto?> ObtenerPersonajePorIdAsync(int id)
        {
            var personaje = await _context.Personajes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (personaje == null)
            {
                _logger.LogWarning("No se encontro el personaje con ID: {PersonajeId}", id);
                return null;
            }

            _logger.LogInformation("Se consulto el personaje con ID: {PersonajeId}", id);

            return new PersonajeDto
            {
                Id = personaje.Id,
                Nombre = personaje.Nombre,
                Especie = personaje.Especie,
                Faccion = personaje.Faccion.ToString(),
                Afiliacion = personaje.Afiliacion,
                Estado = personaje.Estado?.ToString(),
                FuerzaSensitiva = personaje.FuerzaSensitiva,
                ImagenUrl = personaje.ImagenUrl
            };
        }

        public async Task<PersonajeDto> CrearPersonajeAsync(CrearPersonajeDto dto)
        {
            var personaje = new Personaje
            {
                Nombre = dto.Nombre,
                Especie = dto.Especie,
                Faccion = dto.Faccion,
                Afiliacion = dto.Afiliacion,
                Estado = EstadoPersonaje.Vivo,
                FuerzaSensitiva = dto.FuerzaSensitiva,
                ImagenUrl = dto.ImagenUrl
            };

            _context.Personajes.Add(personaje);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Personaje creado correctamente. ID: {PersonajeId}", personaje.Id);

            return new PersonajeDto
            {
                Id = personaje.Id,
                Nombre = personaje.Nombre,
                Especie = personaje.Especie,
                Faccion = personaje.Faccion.ToString(),
                Afiliacion = personaje.Afiliacion,
                Estado = personaje.Estado?.ToString(),
                FuerzaSensitiva = personaje.FuerzaSensitiva,
                ImagenUrl = personaje.ImagenUrl
            };
        }

        public async Task<bool> ActualizarPersonajeAsync(int Id, ActualizarPersonajeDto dto)
        {
            var personaje = await _context.Personajes.FirstOrDefaultAsync(p => p.Id == Id);
            if (personaje == null)
            {
                _logger.LogWarning("No se pudo actualizar. No existe el personaje con ID: {PersonajeId}", Id);
                return false;
            }

            personaje.Nombre = dto.Nombre;
            personaje.Especie = dto.Especie;
            personaje.Faccion = dto.Faccion;
            personaje.Afiliacion = dto.Afiliacion;
            personaje.Estado = dto.Estado;
            personaje.FuerzaSensitiva = dto.FuerzaSensitiva;
            personaje.ImagenUrl = dto.ImagenUrl;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Personaje actualizado correctamente. ID: {PersonajeId}", Id);

            return true;
        }

        public async Task<bool> EliminarPersonajeAsync(int Id)
        {
            var personaje = await _context.Personajes.FirstOrDefaultAsync(p => p.Id == Id);
            if (personaje == null)
            {
                _logger.LogWarning("No se pudo eliminar. No existe el personaje con ID: {PersonajeId}", Id);
                return false;
            }

            _context.Personajes.Remove(personaje);
            await _context.SaveChangesAsync();

            _logger.LogWarning("Personaje eliminado. ID: {PersonajeId}", Id);

            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id, CambiarEstadoPersonajeDto dto)
        {
            var personaje = await _context.Personajes.FirstOrDefaultAsync(p => p.Id == id);
            if (personaje is null)
            {
                _logger.LogWarning("No se pudo cambiar el estado. No existe el personaje con ID: {PersonajeId}", id);
                return false;
            }

            personaje.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Estado del personaje actualizado. ID: {PersonajeId}, Nuevo estado: {Estado}", id, dto.Estado);

            return true;
        }
    }
}