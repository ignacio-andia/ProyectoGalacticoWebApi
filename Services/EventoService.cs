using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;
using System.Diagnostics;
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


        private double GenerarFactorAleatorio()
        {
            return Random.Shared.NextDouble() * (1.10 - 0.90) + 0.90;
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
            var personaje = await _context.Personajes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == personajeId);
            if (personaje is null)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "El personaje no existe");
            }

            var tieneCard = await _context.CardsPersonajes.AsNoTracking().AnyAsync(c => c.PersonajeId == personajeId);
            if (!tieneCard)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "El personaje no tiene Card");
            }

            var existeEvento = await _context.Eventos.AsNoTracking().AnyAsync(e => e.Id == eventoId);
            if (!existeEvento)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.NotFound, "El evento no existe");
            }

            if (personaje.Estado == EstadoPersonaje.Muerto)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.BadRequest, "El personaje está muerto");
            }

            var yaParticipa = await _context.ParticipaEventos.AsNoTracking().AnyAsync(y => y.IdPersonaje == personajeId && y.IdEvento == eventoId);
            if (yaParticipa)
            {
                return ResultadoOperacion<bool>.Error(TipoResultado.Conflict, "El personaje ya participa en el evento");
            }

            var participacion = new ParticipaEvento
            {
                IdPersonaje = personajeId,
                IdEvento = eventoId
            };

            _context.ParticipaEventos.Add(participacion);
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

        public async Task<IEnumerable<PersonajeDto>?> ListarParticipantesAsync(int eventoId)
        {
            var existeEvento = await _context.Eventos.AsNoTracking().AnyAsync(e => e.Id == eventoId);
            if (!existeEvento)
            {
                return null;
            }

            var participantes = await _context.ParticipaEventos.AsNoTracking().Where(pe => pe.IdEvento == eventoId).Select(pe => new PersonajeDto
            {
                Id = pe.Personaje.Id,
                Nombre = pe.Personaje.Nombre,
                Especie = pe.Personaje.Especie,
                Faccion = pe.Personaje.Faccion.ToString(),
                Afiliacion = pe.Personaje.Afiliacion,
                Estado = pe.Personaje.Estado.ToString(),
                FuerzaSensitiva = pe.Personaje.FuerzaSensitiva,
                ImagenUrl = pe.Personaje.ImagenUrl
            })
                .ToListAsync();

            return participantes;
        }

        public async Task<IEnumerable<EventoDto>?> ObtenerEventosPorPersonajeAsync(int personajeId)
        {
            var existePersonaje = await _context.Personajes.AsNoTracking().AnyAsync(p => p.Id == personajeId);
            if (!existePersonaje)
            {
                return null;
            }

            var eventos = await _context.ParticipaEventos.AsNoTracking().Where(e => e.IdPersonaje == personajeId).Include(e => e.Evento).Select(e => new EventoDto
            {
                Id = e.Evento.Id,
                Nombre = e.Evento.Nombre,
                Fecha = e.Evento.Fecha,
                Ubicacion = e.Evento.Ubicacion,
                Descripcion = e.Evento.Descripcion,
                Resultado = e.Evento.Resultado
            }).ToListAsync();

            return eventos;
        }

        public async Task<PersonajeDto?> ObtenerMVPAsync(int eventoId)
        {
            var evento = await _context.Eventos.AsNoTracking().FirstOrDefaultAsync(e => e.Id == eventoId);

            if (evento is null || evento.Resultado is null)
            {
                return null;
            }

            var participanteMvp = await _context.ParticipaEventos.AsNoTracking().Where(pe => pe.IdEvento == eventoId && pe.Personaje.Estado == EstadoPersonaje.Vivo && pe.Personaje.Faccion != Faccion.Neutral).Include(pe => pe.Personaje).ThenInclude(p => p.CardsPersonaje).OrderByDescending(pe => pe.Personaje.CardsPersonaje.Poder).FirstOrDefaultAsync();
            if (participanteMvp is null)
            {
                return null;
            }

            return new PersonajeDto
            {
                Id = participanteMvp.Personaje.Id,
                Nombre = participanteMvp.Personaje.Nombre,
                Especie = participanteMvp.Personaje.Especie,
                Faccion = participanteMvp.Personaje.Faccion.ToString(),
                Afiliacion = participanteMvp.Personaje.Afiliacion,
                Estado = participanteMvp.Personaje.Estado.ToString(),
                FuerzaSensitiva = participanteMvp.Personaje.FuerzaSensitiva,
                ImagenUrl = participanteMvp.Personaje.ImagenUrl
            };
        }

        public async Task<ResultadoOperacion<SimulacionEventoDto>> SimularEventoAsync(int eventoId)
        {
            //verificar si exite y el evento no fue simulado antes
            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == eventoId );
            if (evento is null){
                return ResultadoOperacion<SimulacionEventoDto>.Error(TipoResultado.NotFound, "No exixste Este Evento");
            }
            if(evento.Resultado is not null)
            {
                return ResultadoOperacion<SimulacionEventoDto>.Error(TipoResultado.Conflict, "Este evento ya Fue simulado");
            }


            //validar Participantes (minimo unuo de cada faccion)
            var participantes = await _context.ParticipaEventos.Where(pe => pe.IdEvento == eventoId && pe.Personaje.Estado == EstadoPersonaje.Vivo).Include(pe => pe.Personaje).ThenInclude(p => p.CardsPersonaje).ToListAsync();

            var hayRebeldes = participantes.Any(p => p.Personaje.Faccion == Faccion.Rebelde); 
            var hayImperiales = participantes.Any(p => p.Personaje.Faccion == Faccion.Imperio); 

            if(!hayImperiales || !hayRebeldes)
            {
                return ResultadoOperacion<SimulacionEventoDto>.Error(TipoResultado.BadRequest, "el Eventod ebe de tener un participante Rebelde y uno imperial Minimamte");
            }

            //verificar si tiene cards
            var faltaCard = participantes.Any(p => p.Personaje.CardsPersonaje is null);
            if (faltaCard)
            {
                return ResultadoOperacion<SimulacionEventoDto>.Error(
                    TipoResultado.BadRequest,
                    "Todos los participantes deben tener una Card para simular el evento.");
            }


            // sumar Fuerzas
            var fuerzaRebeldes = participantes.Where(p => p.Personaje.Faccion == Faccion.Rebelde).Sum(p => p.Personaje.CardsPersonaje.Poder);
            var fuerzaImperio = participantes.Where(p => p.Personaje.Faccion == Faccion.Imperio).Sum(p => p.Personaje.CardsPersonaje.Poder);

            //Factor para cada faccion
            double factorRebelde = GenerarFactorAleatorio();
            double factorImperio = GenerarFactorAleatorio();

            //Calcular Fuerza final
            double poderFinalRebelde = fuerzaRebeldes * factorRebelde;
            double poderFinalImperio = fuerzaImperio * factorImperio;


            //verificar ganador
            Faccion ganador;

            if (poderFinalRebelde > poderFinalImperio)
            {
                ganador = Faccion.Rebelde;
            }
            else if (poderFinalImperio > poderFinalRebelde)
            {
                ganador = Faccion.Imperio;
            }
            else
            {// en caso de que deuqen empatos 
                ganador = Random.Shared.Next(2) == 0 ? Faccion.Rebelde: Faccion.Imperio;
            }


            //Marcar Como mueetos a Perdedores
            foreach (var participante in participantes)
            {
                if(participante.Personaje.Faccion != ganador && participante.Personaje.Faccion != Faccion.Neutral)
                {
                    participante.Personaje.Estado = EstadoPersonaje.Muerto;
                }
            }

            //Guaradr Datos en Bdd
            evento.Resultado = $"Victoria de {ganador}";

            await _context.SaveChangesAsync();

            //MostrarDatos
            var respuesta = new SimulacionEventoDto
            {
                EventoId = evento.Id,
                NombreEvento = evento.Nombre,

                FuerzaBaseRebeldes = fuerzaRebeldes,
                FuerzaBaseImperio = fuerzaImperio,

                FactorRebelde = factorRebelde,
                FactorImperio = factorImperio,

                FuerzaFinalRebeldes = poderFinalRebelde,
                FuerzaFinalImperio = poderFinalImperio,

                Resultado = evento.Resultado,
                Criterio = "Gana la facción con mayor fuerza final después de aplicar un factor aleatorio independiente entre 0.90 y 1.10. Los participantes de la facción perdedora mueren; los neutrales no participan en el combate."
            };
            return ResultadoOperacion<SimulacionEventoDto>.Exito(respuesta);

        }

    }
}
