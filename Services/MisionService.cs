using Microsoft.EntityFrameworkCore;
using ProyectoGalacticoWebApi.Data;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Models;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

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


        public async Task<ResultadoOperacion<EjecucionMisionDto>> IniciarMisionAsync(int misionId)
        {
            //ver si mision existe y esta disponible
            var mision = await _context.Misiones.FirstOrDefaultAsync(m => m.Id == misionId);
            if (mision is null)
            {
                return ResultadoOperacion<EjecucionMisionDto>.Error(TipoResultado.NotFound, "La misión no existe");
            }
            if (mision.Estado != EstadoMision.Disponible)
            {
                return ResultadoOperacion<EjecucionMisionDto>.Error(TipoResultado.Conflict, "La misión ya fue ejecutada o no está disponible");
            }

            //ver si hay participantes en la mision
            var participantes = await _context.ParticipaMisiones.Where(pm => pm.IdMision == misionId).Include(pm => pm.Personaje).ThenInclude(p => p.CardsPersonaje).ToListAsync();
            if (!participantes.Any())
            {
                return ResultadoOperacion<EjecucionMisionDto>.Error(TipoResultado.BadRequest, "La misión no tiene participantes");
            }

            //verificar que todos los participantes tengan Card
            if (participantes.Any(p => p.Personaje.CardsPersonaje is null))
            {
                return ResultadoOperacion<EjecucionMisionDto>.Error(TipoResultado.BadRequest, "Uno o más participantes no tienen Card");
            }

            //validar si cumplen con el poder minimo
            var poderTotal = participantes.Sum(p => p.Personaje.CardsPersonaje.Poder);
            if (poderTotal < mision.PoderMinimo)
            {
                return ResultadoOperacion<EjecucionMisionDto>.Error(TipoResultado.BadRequest, "El poder total de los participantes no alcanza el mínimo requerido para esta misión");
            }

            //calculos de probabilidad de exito
            double probabilidadExito = 50 + (((double)(poderTotal - mision.PoderMinimo) / mision.PoderMinimo) * 20) - (mision.NivelDeDificultad * 5);
            probabilidadExito = Math.Clamp(probabilidadExito, 5, 95);

            bool misionExitosa = Random.Shared.NextDouble() * 100 < probabilidadExito;
            var resultado = misionExitosa ? ResultadoMision.Exitosa : ResultadoMision.Fallida;

            //inicio de transaccion
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                //segun el resultado los personajes o quedan vivos, heridos o muertos
                foreach (var participante in participantes)
                {
                    if (!misionExitosa)
                    {
                        participante.Personaje.Estado = EstadoPersonaje.Muerto;
                    }
                    else
                    {
                        int probabilidad = Random.Shared.Next(1, 101);

                        participante.Personaje.Estado = probabilidad <= 70
                            ? EstadoPersonaje.Vivo
                            : EstadoPersonaje.Herido;
                    }
                }

                //distribuir recompensa si la mision es completada exitosamente
                var participantesVivos = participantes.Where(p => p.Personaje.Estado == EstadoPersonaje.Vivo || p.Personaje.Estado == EstadoPersonaje.Herido).ToList();
                int recompensaDistribuida = 0;

                var monedasPorParticipante = new Dictionary<int, int>();

                if (misionExitosa)
                {
                    int recompensaIndividual = mision.Recompensa / participantesVivos.Count;
                    int sobrante = mision.Recompensa % participantesVivos.Count;

                    for (int i = 0; i < participantesVivos.Count; i++)
                    {
                        int monedasRecibidas = recompensaIndividual;

                        if (i < sobrante)
                        {
                            monedasRecibidas++;
                        }

                        participantesVivos[i].Personaje.Monedas += monedasRecibidas;

                        monedasPorParticipante[participantesVivos[i].Personaje.Id] = monedasRecibidas;

                        recompensaDistribuida += monedasRecibidas;
                    }
                }

                //actualizar estado y resultado de la mision
                mision.Estado = EstadoMision.Finalizada;
                mision.Resultado = resultado;

                //crear el DTO para mostrar el resultado
                var ejecucionDto = new EjecucionMisionDto
                {
                    MisionId = mision.Id,
                    NombreMision = mision.Nombre,
                    Resultado = resultado.ToString(),
                    PoderTotal = poderTotal,
                    PoderMinimo = mision.PoderMinimo,
                    ProbabilidadExito = probabilidadExito,
                    RecompensaTotal = mision.Recompensa,
                    RecompensaDistribuida = recompensaDistribuida,
                    Participantes = participantes.Select(p => new ParticipanteEjecucionMisionDto
                    {
                        PersonajeId = p.Personaje.Id,
                        Nombre = p.Personaje.Nombre,
                        EstadoFinal = p.Personaje.Estado!.Value.ToString(),
                        MonedasRecibidas = monedasPorParticipante.GetValueOrDefault(p.Personaje.Id, 0)
                    }).ToList()
                };

                await _context.SaveChangesAsync();
                await transaccion.CommitAsync();

                return ResultadoOperacion<EjecucionMisionDto>.Exito(ejecucionDto);
            }
            catch
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

    }
}
