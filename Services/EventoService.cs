using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Services.Interfaces;
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
    }
}
