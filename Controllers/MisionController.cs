using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;
using ProyectoGalacticoWebApi.Services.Interfaces;

namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MisionController : ControllerBase
    {
        private readonly IMisionService _misionService;

        public MisionController(IMisionService misionService)
        {
            _misionService = misionService;
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

        // GET: api/misiones
        [HttpGet]
        public async Task<IActionResult> ListarMisiones()
        {
            var misiones = await _misionService.ListarMisionesAsync();
            return Ok(misiones);
        }

        // POST: api/misiones
        [HttpPost]
        public async Task<IActionResult> CrearMision(CrearMisionDto dto)
        {
            var resultado = await _misionService.CrearMisionAsync(dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return StatusCode(201, resultado.Datos);
        }

        // POST: api/misiones/{id}/participantes/{personajeId}
        [HttpPost("{id:int}/participantes/{personajeId:int}")]
        public async Task<IActionResult> AgregarParticipante(int id, int personajeId)
        {
            var resultado = await _misionService.AgregarParticipanteAsync(id, personajeId);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return NoContent();
        }

        // POST: api/misiones/{id}/iniciar
        [HttpPost("{id:int}/iniciar")]
        public async Task<IActionResult> IniciarMision(int id)
        {
            var resultado = await _misionService.IniciarMisionAsync(id);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return Ok(resultado.Datos);
        }
    }
}
