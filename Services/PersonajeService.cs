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
        public PersonajeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonajeDto>> ListarPersonajesAsync()
        {
            var personajes = await _context.Personajes.AsNoTracking().ToListAsync();

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
            if(personaje == null)
            {
                return null;
            }
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
            return true;    
        }

        public async Task<bool> EliminarPersonajeAsync(int Id)
        {
            var personaje = await _context.Personajes.FirstOrDefaultAsync(p => p.Id == Id);
            if (personaje == null)
            {
                return false;
            }
            _context.Personajes.Remove(personaje);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id, CambiarEstadoPersonajeDto dto)
        {
            var personaje = await _context.Personajes.FirstOrDefaultAsync(p => p.Id == id);
            if (personaje is null)
            {
                return false;
            }

            personaje.Estado = dto.Estado;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
