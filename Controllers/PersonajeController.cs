using Microsoft.AspNetCore.Mvc;
using ProyectoGalacticoWebApi.Dtos;
using ProyectoGalacticoWebApi.Services.Interfaces;

namespace ProyectoGalacticoWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonajeController : ControllerBase
    {
        private readonly IPersonajeService _personajeService;
        public PersonajeController(IPersonajeService personajeService)
        {
            _personajeService = personajeService;
        }

        [HttpGet]
        [EndpointSummary("Listar todos los personajes")]
        [EndpointDescription("""
        Obtiene la lista completa de personajes registrados en el catalogo galactico.

        FUNCIONAMIENTO:
        - Consulta los personajes almacenados en la base de datos.
        - Devuelve la informacion de los personajes encontrados.

        RESPUESTAS:
        - 200 OK: lista de personajes obtenida correctamente.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ListarPersonajes()
        {
            var personajes = await _personajeService.ListarPersonajesAsync();
            return Ok(personajes);
        }

        [HttpGet("{Id}")]
        [EndpointSummary("Obtener un personaje por ID")]
        [EndpointDescription("""
        Busca y devuelve un personaje utilizando su identificador.

        PARAMETROS:
        - Id: identificador unico del personaje.

        FUNCIONAMIENTO:
        - Busca el personaje en la base de datos.
        - Si existe, devuelve toda la informacion disponible.
        - Si no existe, devuelve un error indicando que no fue encontrado.

        RESPUESTAS:
        - 200 OK: personaje encontrado correctamente.
        - 404 Not Found: no existe un personaje con ese identificador.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ObtenerPersonajePorId(int Id)
        {
            var personaje = await _personajeService.ObtenerPersonajePorIdAsync(Id);
            if (personaje == null)
            {
                return NotFound();
            }
            return Ok(personaje);
        }

        [HttpPost]
        [EndpointSummary("Crear un nuevo personaje")]
        [EndpointDescription("""
        Registra un nuevo personaje en el catalogo galactico.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los datos definidos en CrearPersonajeDto.
        - El identificador es generado por la base de datos.

        FUNCIONAMIENTO:
        - Recibe los datos del nuevo personaje.
        - Delega la creacion al servicio de personajes.
        - Guarda el nuevo registro en la base de datos.
        - Devuelve el personaje creado y su ubicacion para consultarlo.

        RESPUESTAS:
        - 201 Created: personaje creado correctamente.
        - 400 Bad Request: los datos enviados no cumplen las validaciones.
        """)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CrearPersonaje(CrearPersonajeDto dto)
        {
            var personaje = await _personajeService.CrearPersonajeAsync(dto);
            return CreatedAtAction(nameof(ObtenerPersonajePorId), new { id = personaje.Id }, personaje); //Para el 221
        }

        [HttpPut("{id}")]
        [EndpointSummary("Actualizar un personaje")]
        [EndpointDescription("""
        Actualiza la informacion de un personaje existente.

        PARAMETROS:
        - id: identificador del personaje que se desea modificar.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los nuevos datos definidos en ActualizarPersonajeDto.

        FUNCIONAMIENTO:
        - Busca el personaje mediante su identificador.
        - Si existe, actualiza su informacion.
        - Si no existe, no realiza la modificacion.

        RESPUESTAS:
        - 204 No Content: personaje actualizado correctamente.
        - 400 Bad Request: los datos enviados no cumplen las validaciones.
        - 404 Not Found: no existe un personaje con ese identificador.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ActualizarPersonaje(int id, ActualizarPersonajeDto dto)
        {
            var personaje = await _personajeService.ActualizarPersonajeAsync(id, dto);
            if (!personaje)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Eliminar un personaje")]
        [EndpointDescription("""
        Elimina un personaje del catalogo galactico mediante su identificador.

        PARAMETROS:
        - id: identificador del personaje que se desea eliminar.

        FUNCIONAMIENTO:
        - Busca el personaje en la base de datos.
        - Si existe, solicita su eliminacion.
        - Si no existe, devuelve un error indicando que no fue encontrado.

        RESPUESTAS:
        - 204 No Content: personaje eliminado correctamente.
        - 404 Not Found: no existe un personaje con ese identificador.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> EliminarPersonaje(int id)
        {
            var personaje = await _personajeService.EliminarPersonajeAsync(id);
            if (!personaje)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPatch("{id:int}/estado")]
        [EndpointSummary("Cambiar el estado de un personaje")]
        [EndpointDescription("""
        Modifica unicamente el estado actual de un personaje.

        PARAMETROS:
        - id: identificador del personaje.

        DATOS DE LA SOLICITUD:
        - Se debe enviar el nuevo estado mediante CambiarEstadoPersonajeDto.

        FUNCIONAMIENTO:
        - Busca el personaje mediante su identificador.
        - Si existe, actualiza su estado.
        - Si no existe, devuelve un error indicando que no fue encontrado.

        RESPUESTAS:
        - 204 No Content: estado actualizado correctamente.
        - 400 Bad Request: el estado enviado no cumple las validaciones.
        - 404 Not Found: no existe un personaje con ese identificador.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CambiarEstado(int id, CambiarEstadoPersonajeDto dto)
        {
            var resultado = await _personajeService.CambiarEstadoAsync(id, dto);
            if (!resultado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}