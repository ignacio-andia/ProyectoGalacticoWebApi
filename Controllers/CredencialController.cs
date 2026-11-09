using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Services.Interfaces;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Results;

namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredencialController : ControllerBase
    {
        private readonly ICredencialService _credencialService;

        public CredencialController(ICredencialService credencialService)
        {
            _credencialService = credencialService;
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

        // POST: api/Credencial/
        [HttpPost("{personajeId:int}")]
        public async Task<IActionResult> CrearCredencial(int personajeId, CrearCredencialDto dto)
        {
            var resultado = await _credencialService.CrearCredencialAsync(personajeId, dto);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return CreatedAtAction(
                nameof(ObtenerCredencial),
                new { personajeId },
                resultado.Datos);
        }

        // GET: api/Credencial/
        [HttpGet("{personajeId:int}")]
        public async Task<IActionResult> ObtenerCredencial(int personajeId)
        {
            var resultado = await _credencialService.ObtenerCredencialAsync(personajeId);

            if (resultado.Tipo != TipoResultado.Success)
            {
                return ManejarResultado(resultado);
            }

            return Ok(resultado.Datos);
        }
    }
}