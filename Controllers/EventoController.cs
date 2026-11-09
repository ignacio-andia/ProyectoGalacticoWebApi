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
        [EndpointSummary("Listar todos los eventos")]
        [EndpointDescription("""
        Obtiene la lista completa de eventos registrados en el catalogo galactico.

        FUNCIONAMIENTO:
        - Consulta los eventos almacenados en la base de datos.
        - Devuelve la informacion de cada evento.
        - Incluye los datos disponibles de los eventos registrados.

        RESPUESTAS:
        - 200 OK: lista de eventos obtenida correctamente.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarEventos()
        {
            var eventos = await _eventoService.ListarEventosAsync();
            return Ok(eventos);
        }

        [HttpGet("{id:int}")]
        [EndpointSummary("Obtener un evento por ID")]
        [EndpointDescription("""
        Busca y devuelve un evento mediante su identificador.

        PARAMETROS:
        - id: identificador unico del evento.

        FUNCIONAMIENTO:
        - Busca el evento en la base de datos.
        - Si existe, devuelve su informacion.
        - Si no existe, devuelve un error indicando que no fue encontrado.

        RESPUESTAS:
        - 200 OK: evento encontrado correctamente.
        - 404 Not Found: no existe un evento con ese identificador.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Crear un nuevo evento")]
        [EndpointDescription("""
        Registra un nuevo evento en el catalogo galactico.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los datos definidos en CrearEventoDto.
        - Incluye el nombre, fecha, ubicacion y descripcion del evento.

        FUNCIONAMIENTO:
        - Recibe los datos del nuevo evento.
        - Delega las validaciones y la creacion al servicio.
        - Si la operacion es exitosa, guarda el evento en la base de datos.
        - Devuelve el evento creado y su ubicacion para consultarlo.

        RESPUESTAS:
        - 201 Created: evento creado correctamente.
        - 400 Bad Request: los datos enviados no son validos.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [EndpointSummary("Actualizar un evento")]
        [EndpointDescription("""
        Actualiza la informacion de un evento existente.

        PARAMETROS:
        - id: identificador del evento que se desea modificar.

        DATOS DE LA SOLICITUD:
        - Se deben enviar los nuevos datos definidos en ActualizarEventoDto.

        FUNCIONAMIENTO:
        - Busca el evento mediante su identificador.
        - Delega las validaciones y la actualizacion al servicio.
        - Si la operacion es exitosa, guarda los cambios en la base de datos.

        RESPUESTAS:
        - 204 No Content: evento actualizado correctamente.
        - 400 Bad Request: los datos enviados no son validos.
        - 404 Not Found: no se encontro el evento solicitado.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [EndpointSummary("Agregar un participante a un evento")]
        [EndpointDescription("""
        Registra la participacion de un personaje en un evento.

        PARAMETROS:
        - eventoId: identificador del evento.
        - personajeId: identificador del personaje que se desea agregar.

        FUNCIONAMIENTO:
        - Recibe los identificadores del evento y del personaje.
        - Delega las validaciones y el registro de la participacion al servicio.
        - Si la operacion es exitosa, registra la relacion entre ambos.

        RESPUESTAS:
        - 204 No Content: participante agregado correctamente.
        - 400 Bad Request: la solicitud no cumple las validaciones.
        - 404 Not Found: no se encontro el evento o el personaje.
        - 409 Conflict: existe un conflicto con la participacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [EndpointSummary("Quitar un participante de un evento")]
        [EndpointDescription("""
        Elimina la participacion de un personaje en un evento.

        PARAMETROS:
        - eventoId: identificador del evento.
        - personajeId: identificador del personaje que se desea retirar.

        FUNCIONAMIENTO:
        - Recibe los identificadores del evento y del personaje.
        - Delega la validacion y eliminacion de la participacion al servicio.
        - Si la operacion es exitosa, elimina la relacion entre ambos.

        RESPUESTAS:
        - 204 No Content: participante retirado correctamente.
        - 400 Bad Request: la solicitud no cumple las validaciones.
        - 404 Not Found: no se encontro el evento o la participacion.
        - 409 Conflict: existe un conflicto con la operacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
        [EndpointSummary("Listar los participantes de un evento")]
        [EndpointDescription("""
        Obtiene la lista de personajes registrados como participantes de un evento.

        PARAMETROS:
        - eventoId: identificador del evento cuyos participantes se desean consultar.

        FUNCIONAMIENTO:
        - Busca los participantes asociados al evento.
        - Devuelve la informacion de los personajes registrados.
        - Si no se encuentra el evento, devuelve un error.

        RESPUESTAS:
        - 200 OK: lista de participantes obtenida correctamente.
        - 404 Not Found: no se encontro el evento solicitado.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Consultar los eventos de un personaje")]
        [EndpointDescription("""
        Obtiene los eventos en los que se encuentra registrado un personaje.

        PARAMETROS:
        - personajeId: identificador del personaje.

        FUNCIONAMIENTO:
        - Busca los eventos relacionados con el personaje indicado.
        - Devuelve la informacion de los eventos encontrados.
        - Si no se encuentra el personaje o no se obtiene informacion,
          devuelve un error.

        RESPUESTAS:
        - 200 OK: eventos del personaje obtenidos correctamente.
        - 404 Not Found: no se encontro el personaje o la informacion solicitada.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Obtener el MVP de un evento")]
        [EndpointDescription("""
        Obtiene el personaje destacado de un evento.

        PARAMETROS:
        - eventoId: identificador del evento.

        FUNCIONAMIENTO:
        - Solicita al servicio la determinacion del MVP del evento.
        - Devuelve la informacion del personaje seleccionado.
        - Si no se encuentra un MVP disponible, devuelve un error.

        RESPUESTAS:
        - 200 OK: MVP obtenido correctamente.
        - 404 Not Found: no se encontro el evento o no existe un MVP disponible.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [EndpointSummary("Simular un evento")]
        [EndpointDescription("""
        Ejecuta la simulacion de un enfrentamiento entre las facciones participantes de un evento.

        PARAMETROS:
        - eventoId: identificador del evento que se desea simular.

        REQUISITOS PARA LA SIMULACION:
        - El evento debe existir y no haber sido simulado anteriormente.
        - Debe existir al menos un personaje vivo de la faccion Rebelde.
        - Debe existir al menos un personaje vivo de la faccion Imperial.
        - Todos los participantes vivos deben tener una card registrada.

        FUNCIONAMIENTO:
        - Obtiene los participantes vivos del evento.
        - Calcula el poder total de cada faccion utilizando las cards.
        - Aplica un factor aleatorio al poder de cada faccion.
        - Determina la faccion ganadora.
        - En caso de empate, selecciona un ganador aleatoriamente.
        - Los personajes de la faccion perdedora mueren.
        - Los personajes neutrales no mueren por el resultado del enfrentamiento.
        - Guarda el resultado y los cambios de estado en la base de datos.

        RESPUESTAS:
        - 200 OK: simulacion ejecutada correctamente.
        - 400 Bad Request: no se cumplen los requisitos para ejecutar la simulacion.
        - 404 Not Found: no se encontro el evento solicitado.
        - 409 Conflict: el evento ya fue simulado anteriormente.
        """)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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