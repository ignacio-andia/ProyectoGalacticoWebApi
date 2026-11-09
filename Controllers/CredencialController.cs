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

        [HttpPost("{personajeId:int}")]
        [EndpointSummary("Crear credenciales para un personaje")]
        [EndpointDescription("""
        Registra las credenciales de acceso asociadas a un personaje existente.

        PARAMETROS:
        - personajeId: identificador del personaje al que se asociaran las credenciales.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los datos definidos en CrearCredencialDto.
        - CodigoAcceso: codigo de acceso que se almacenara mediante hash.
        - CanalDeComunicacion: canal de comunicacion que se almacenara cifrado.

        FUNCIONAMIENTO:
        - Verifica que el personaje exista.
        - Comprueba que el personaje no tenga credenciales registradas previamente.
        - Genera un hash del codigo de acceso mediante PasswordHasher.
        - Cifra el canal de comunicacion mediante el servicio de cifrado.
        - Guarda las credenciales en la base de datos.
        - Devuelve los datos de las credenciales creadas, sin exponer el hash del codigo de acceso.

        RESPUESTAS:
        - 201 Created: credenciales creadas correctamente.
        - 400 Bad Request: los datos enviados no son validos.
        - 404 Not Found: no existe el personaje indicado.
        - 409 Conflict: el personaje ya tiene credenciales registradas.
        """)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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

        [HttpGet("{personajeId:int}")]
        [EndpointSummary("Obtener las credenciales de un personaje")]
        [EndpointDescription("""
        Obtiene las credenciales registradas para un personaje mediante su identificador.

        PARAMETROS:
        - personajeId: identificador del personaje cuyas credenciales se desean consultar.

        FUNCIONAMIENTO:
        - Busca las credenciales asociadas al personaje.
        - Recupera el canal de comunicacion almacenado en la base de datos.
        - Descifra el canal de comunicacion mediante el servicio de cifrado.
        - Devuelve los datos de las credenciales.
        - El hash del codigo de acceso nunca se incluye en la respuesta.

        PROTECCION DE DATOS:
        - El codigo de acceso se almacena mediante hash y no puede recuperarse como texto original.
        - El canal de comunicacion se almacena cifrado y se descifra al consultar las credenciales.

        RESPUESTAS:
        - 200 OK: credenciales obtenidas correctamente.
        - 404 Not Found: no existen credenciales para el personaje indicado.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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