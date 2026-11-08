using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Services.Interfaces;
using System.Security.Cryptography.X509Certificates;
namespace ProyectoGalacticoWebApi.Services
{
    public class EventoService : IEventoService
    {
        private readonly AppDbContext _context;
        public EventoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventoDto>> ListarEventosAsync()
        {
            var eventos = await _context.Eventos.AsNoTracking().ToListAsync();
            return eventos.Select(e => new EventoDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Fecha = e.Fecha,
                Ubicacion = e.Ubicacion, 
                Descripcion = e.Descripcion,
                Resultado = e.Resultado
            });
        }

        public async Task<EventoDto?> ObtenerEventoPorIdAsync(int id)
        {
            var evento = await _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (evento is null)
            {
                return null;
            }

            return new EventoDto
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                Fecha = evento.Fecha,
                Ubicacion = evento.Ubicacion,
                Descripcion = evento.Descripcion,
                Resultado = evento.Resultado
            };
        }

        public async Task<ResultadoOperacion<EventoDto>> CrearEventoAsync(CrearEventoDto dto)
        {
            var evento = new Evento
            {
                Nombre = dto.Nombre,
                Fecha = dto.Fecha,
                Ubicacion = dto.Ubicacion,
                Descripcion = dto.Descripcion
            };
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            var eventoDto = new EventoDto
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                Fecha = evento.Fecha,
                Ubicacion = evento.Ubicacion,
                Descripcion = evento.Descripcion,
                Resultado = evento.Resultado
            };
            return ResultadoOperacion<EventoDto>.Exito(eventoDto);
        }

        public async Task<ResultadoOperacion<bool>> ActualizarEventoAsync(int id, ActualizarEventoDto dto)
        {
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);
            if(evento is null)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "El Evento Indicado no existe.");
            }
            evento.Nombre = dto.Nombre;
            evento.Fecha = dto.Fecha;
            evento.Ubicacion = dto.Ubicacion;
            evento.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();

            return ResultadoOperacion<bool>.Exito(true);


        }

        public async Task<ResultadoOperacion<bool>> AgregarParticipanteAsync(int eventoId, int personajeId)
        {
            var ExistePersonaje = await _context.Personajes.AsNoTracking().AnyAsync(p => p.Id == personajeId);
            if (!ExistePersonaje)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "El Personaje No Existe");
            }

            var tieneCard = await _context.CardsPersonajes.AsNoTracking().AnyAsync(c => c.PersonajeId == personajeId);
            if (!tieneCard)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "El personaje No tiene Card");
            }

            var ExisteEvento = await _context.Eventos.AsNoTracking().AnyAsync(e => e.Id == eventoId);
            if (!ExisteEvento)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "Noexiste Evento");
            }

            var personaje = await _context.Personajes.AsNoTracking().FirstOrDefaultAsync(pe => pe.Id == personajeId);
            if (personaje is null)
            {
                return ResultadoOperacion<bool>.Error(
                    TipoResultado.NotFound,"El personaje indicado no existe.");
            }
            if (personaje.Estado == EstadoPersonaje.Muerto)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "El personaje esta Muerto");
            }

            var YaParticipa = await _context.ParticipaEventos.AsNoTracking().AnyAsync(y => y.IdPersonaje == personajeId && y.IdEvento == eventoId);
            if (YaParticipa)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.Conflict, "El personaja ya Participa en el Evento");

            }

            var Participacion = new ParticipaEvento
            {
                IdPersonaje = personajeId,
                IdEvento = eventoId
            };

            _context.ParticipaEventos.Add(Participacion);
            await _context.SaveChangesAsync();

            return ResultadoOperacion<bool>.Exito(true);
        }

        public async Task<ResultadoOperacion<bool>> QuitarParticipanteAsync(int eventoId, int personajeId)
        {
            var ExistePersonaje = await _context.Personajes.AsNoTracking().AnyAsync(p => p.Id == personajeId);
            if (!ExistePersonaje)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "El Personaje No Existe");
            }

            var ExisteEvento = await _context.Eventos.AsNoTracking().AnyAsync(e => e.Id == eventoId);
            if (!ExisteEvento)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "Noexiste Evento");
            }

            var Relacion = await _context.ParticipaEventos.FirstOrDefaultAsync(y => y.IdPersonaje == personajeId && y.IdEvento == eventoId);
            if (Relacion is null)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "No existe Esta Relacion");
            }

            _context.ParticipaEventos.Remove(Relacion);
            await _context.SaveChangesAsync();
            return ResultadoOperacion<bool>.Exito(true);
        }
    }
}
