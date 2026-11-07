using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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
                Faccion = p.Faccion,
                Afiliacion = p.Afiliacion,
                Estado = p.Estado,
                FuerzaSensitiva = p.FuerzaSensitiva,
                ImagenUrl = p.ImagenUrl
            });
        }

        public async Task<PersonajeDto?> ObtenerPersonajePorIdAsync(int Id)
        {
            var personaje = await _context.Personajes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);
            if(personaje == null)
            {
                return null;
            }
            return new PersonajeDto
            {
                Id = personaje.Id,
                Nombre = personaje.Nombre,
                Especie = personaje.Especie,
                Faccion = personaje.Faccion,
                Afiliacion = personaje.Afiliacion,
                Estado = personaje.Estado,
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
                Faccion = personaje.Faccion,
                Afiliacion = personaje.Afiliacion,
                Estado = personaje.Estado,
                FuerzaSensitiva = personaje.FuerzaSensitiva,
                ImagenUrl = personaje.ImagenUrl
            };
        }



    }
}
