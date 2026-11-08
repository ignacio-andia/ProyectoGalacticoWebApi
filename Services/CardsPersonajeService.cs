using ProyectoGalacticoWebApi.Services.Interfaces;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace ProyectoGalacticoWebApi.Services
{
    public class CardsPersonajeService : ICardsPersonajeService
    {

        private readonly AppDbContext _context;
        public CardsPersonajeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CardsPersonajeDto>> ListarCardsAsync()
        {
            var cards = await _context.CardsPersonajes.AsNoTracking().ToListAsync();
            return cards.Select(c => new CardsPersonajeDto
            {
                Id = c.Id,
                Poder = c.Poder,
                HabilidadEspecial = c.HabilidadEspecial,
                Arma = c.Arma,
                NivelDePeligrosidad = c.NivelDePeligrosidad,
                PersonajeId = c.PersonajeId
            });
        }

        public async Task<CardsPersonajeDto?> ObtenerCardPorIdAsync(int id)
        {
            var card = await _context.CardsPersonajes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (card == null)
            {
                return null;
            }

            return new CardsPersonajeDto
            {
                Id = card.Id,
                Poder = card.Poder,
                HabilidadEspecial = card.HabilidadEspecial,
                Arma = card.Arma,
                NivelDePeligrosidad = card.NivelDePeligrosidad,
                PersonajeId = card.PersonajeId
            };
        }

        public async Task<CardsPersonajeDto?> ObtenerCardDePersonajeAsync(int id)
        {
            var card = await _context.CardsPersonajes.AsNoTracking().FirstOrDefaultAsync(c => c.PersonajeId == id);
            if (card == null)
            {
                return null;
            }

            return new CardsPersonajeDto
            {
                Id = card.Id,
                Poder = card.Poder,
                HabilidadEspecial = card.HabilidadEspecial,
                Arma = card.Arma,
                NivelDePeligrosidad = card.NivelDePeligrosidad,
                PersonajeId = card.PersonajeId
            };

        }

        public async Task<CardsPersonajeDto> CrearCardAsync(CrearCardsPersonajeDto dto)
        {

            bool personajeExiste = await _context.Personajes.AnyAsync(p => p.Id == dto.PersonajeId);
            if (!personajeExiste)
            {
                throw new KeyNotFoundException("El personaje indicado no existe.");
            }
            bool yaTieneCard = await _context.CardsPersonajes.AnyAsync(c => c.PersonajeId == dto.PersonajeId);
            if (yaTieneCard)
            {
                throw new InvalidOperationException("El personaje ya tiene una Card asignada.");
            }

            var card = new CardsPersonaje
            {
                Poder = dto.Poder,
                HabilidadEspecial = dto.HabilidadEspecial,
                Arma = dto.Arma,
                NivelDePeligrosidad = dto.NivelDePeligrosidad,
                PersonajeId = dto.PersonajeId
            };

            _context.CardsPersonajes.Add(card);
            await _context.SaveChangesAsync();

            return new CardsPersonajeDto
            {
                Id = card.Id,
                Poder = card.Poder,
                HabilidadEspecial = card.HabilidadEspecial,
                Arma = card.Arma,
                NivelDePeligrosidad = card.NivelDePeligrosidad,
                PersonajeId = card.PersonajeId
            };
        }

        public async Task<bool> ActualizarCardAsync(int id, ActualizarCardsPersonajeDto dto)
        {
            var card = await _context.CardsPersonajes.FirstOrDefaultAsync(c => c.Id == id);
            if(card == null)
            {
                return false;
            }

            bool personajeExiste = await _context.Personajes.AnyAsync(p => p.Id == dto.PersonajeId);
            if (!personajeExiste)
            {
                throw new KeyNotFoundException("El personaje indicado no existe.");
            }

            bool yaTieneCard = await _context.CardsPersonajes.AnyAsync(c => c.PersonajeId == dto.PersonajeId && c.Id != id);
            if (yaTieneCard)
            {
                throw new InvalidOperationException("El personaje ya tiene una Card asignada.");
            }

            card.Poder = dto.Poder;
            card.HabilidadEspecial = dto.HabilidadEspecial;
            card.Arma = dto.Arma;
            card.NivelDePeligrosidad = dto.NivelDePeligrosidad;
            card.PersonajeId = dto.PersonajeId;

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> EliminarCardAsync(int id)
        {
            var card = await _context.CardsPersonajes.FirstOrDefaultAsync(c => c.Id == id);
            if (card is null)
            {
                return false;
            }
            _context.CardsPersonajes.Remove(card);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
