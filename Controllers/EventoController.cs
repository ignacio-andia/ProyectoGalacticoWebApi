using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Services.Interfaces;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }
        private IActionResult ManejarResultado<T>(ResultadoOperacion<T> resultado)
        {
            return resultado.Tipo switch
            {
                TipoResultado.NotFound => NotFound(resultado.Mensaje),
                TipoResultado.Conflict => Conflict(resultado.Mensaje),
                TipoResultado.BadRequest => BadRequest(resultado.Mensaje),
                _ => Ok(resultado.Datos)
            };
        }

        [HttpGet]
        public async Task<IActionResult> ListarEventos()
        {
            var eventos = await _eventoService.ListarEventosAsync();
            return Ok(eventos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerEventoPorId(int id)
        {
            var evento = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            return Ok(evento);
        }

        [HttpPost]
        public async Task<IActionResult> CrearEvento(CrearEventoDto dto)
        {
            var resultado = await _eventoService.CrearEventoAsync(dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return CreatedAtAction(
                nameof(ObtenerEventoPorId),
                new { id = resultado.Datos!.Id },
                resultado.Datos);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarEvento(int id, ActualizarEventoDto dto)
        {
            var resultado = await _eventoService.ActualizarEventoAsync(id, dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }


        [HttpPost("{eventoId:int}/participante/{personajeId:int}")]
        public async Task<IActionResult> AgregarParticipante(int eventoId, int personajeId)
        {
            var resultado = await _eventoService.AgregarParticipanteAsync(eventoId, personajeId);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }


        [HttpDelete("{eventoId:int}/participante/{personajeId:int}")]
        public async Task<IActionResult> QuitarParticipante(int eventoId, int personajeId)
        {
            var resultado = await _eventoService.QuitarParticipanteAsync(eventoId, personajeId);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }


        [HttpGet("{eventoId:int}/participantes")]
        public async Task<IActionResult> ListarParticipantes(int eventoId)
        {
            var participantes = await _eventoService.ListarParticipantesAsync(eventoId);

            if (participantes == null)
            {
                return NotFound();
            }

            return Ok(participantes);
        }

        [HttpGet("personaje/{personajeId:int}")]
        public async Task<IActionResult> ObtenerEventosPorPersonaje(int personajeId)
        {
            var eventos = await _eventoService.ObtenerEventosPorPersonajeAsync(personajeId);

            if (eventos == null)
            {
                return NotFound();
            }

            return Ok(eventos);
        }


        [HttpGet("{eventoId:int}/mvp")]
        public async Task<IActionResult> ObtenerMVP(int eventoId)
        {
            var mvp = await _eventoService.ObtenerMVPAsync(eventoId);

            if (mvp == null)
            {
                return NotFound();
            }

            return Ok(mvp);
        }


        [HttpPost("{eventoId:int}/simular")]
        public async Task<IActionResult> SimularEvento(int eventoId)
        {
            var resultado = await _eventoService.SimularEventoAsync(eventoId);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return Ok(resultado.Datos);
        }

    }
}
