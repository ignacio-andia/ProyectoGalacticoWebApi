using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;

namespace ProyectoGalacticoWebApi.Services
{
    public class MisionService : IMisionService
    {
        private readonly AppDbContext _context;

        public MisionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoOperacion<MisionDto>> CrearMisionAsync(CrearMisionDto dto)
        {
            var mision = new Mision
            {
                Nombre = dto.Nombre,
                Tipo = dto.Tipo,
                NivelDeDificultad = dto.NivelDeDificultad,
                Descripcion = dto.Descripcion,
                PoderMinimo = dto.PoderMinimo,
                Recompensa = dto.Recompensa,
                Estado = EstadoMision.Disponible
            };

            _context.Misiones.Add(mision);
            await _context.SaveChangesAsync();

            var misionDto = new MisionDto
            {
                Id = mision.Id,
                Nombre = mision.Nombre,
                Tipo = mision.Tipo,
                NivelDeDificultad = mision.NivelDeDificultad,
                Descripcion = mision.Descripcion,
                PoderMinimo = mision.PoderMinimo,
                Recompensa = mision.Recompensa,
                Estado = mision.Estado.ToString(),
                Resultado = mision.Resultado?.ToString()
            };

            return ResultadoOperacion<MisionDto>.Exito(misionDto);
        }


        public async Task<IEnumerable<MisionDto>> ListarMisionesAsync()
        {
            var misiones = await _context.Misiones.AsNoTracking().ToListAsync();

            return misiones.Select(m => new MisionDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Tipo = m.Tipo,
                NivelDeDificultad = m.NivelDeDificultad,
                Descripcion = m.Descripcion,
                PoderMinimo = m.PoderMinimo,
                Recompensa = m.Recompensa,
                Estado = m.Estado.ToString(),
                Resultado = m.Resultado?.ToString()
            });
        }


        public async Task<ResultadoOperacion<bool>> AgregarParticipanteAsync(int misionId, int personajeId)
        {
            var mision = await _context.Misiones.AsNoTracking().FirstOrDefaultAsync(m => m.Id == misionId);
            if (mision is null)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "La misión no existe");
            }
            if (mision.Estado != EstadoMision.Disponible)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.Conflict, "La misión ya no está disponible para agregar participantes");
            }

            var personaje = await _context.Personajes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == personajeId);
            if (personaje is null)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "El personaje no existe");
            }
            if (personaje.Estado != EstadoPersonaje.Vivo)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "Solo pueden participar personajes vivos");
            }

            var tieneCard = await _context.CardsPersonajes.AsNoTracking().AnyAsync(c => c.PersonajeId == personajeId);
            if (!tieneCard)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "El personaje no tiene Card");
            }

            var yaParticipa = await _context.ParticipaMisiones.AsNoTracking().AnyAsync(pm => pm.IdPersonaje == personajeId && pm.IdMision == misionId);
            if (yaParticipa)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.Conflict, "El personaje ya participa en esta misión");
            }

            var participaEnOtraMision = await _context.ParticipaMisiones.AsNoTracking().AnyAsync(pm => pm.IdPersonaje == personajeId && pm.Mision.Estado == EstadoMision.Disponible);
            if (participaEnOtraMision)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.Conflict, "El personaje ya participa en otra misión que todavía no ha finalizado");
            }

            var faccionExistente = await _context.ParticipaMisiones.AsNoTracking().Where(pm => pm.IdMision == misionId).Select(pm => (Faccion?)pm.Personaje.Faccion).FirstOrDefaultAsync();
            if (faccionExistente.HasValue && faccionExistente.Value != personaje.Faccion)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "Todos los participantes de una misión deben pertenecer a la misma facción");
            }

            var participacion = new ParticipaMision
            {
                IdPersonaje = personajeId,
                IdMision = misionId
            };

            _context.ParticipaMisiones.Add(participacion);
            await _context.SaveChangesAsync();

            return ResultadoOperacion<bool>.Exito(true);
        }

    }
}
