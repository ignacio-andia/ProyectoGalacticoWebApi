# Declaracion de uso de Inteligencia Artificial

## 1. Herramientas utilizadas

Durante el desarrollo del proyecto **Catalogo Galactico - API REST** se utilizo:

- **ChatGPT (OpenAI):** herramienta de asistencia para el aprendizaje, analisis de codigo, resolucion de problemas tecnicos, revision de implementaciones y elaboracion de documentacion.

Se utilizo principalmente como apoyo durante el desarrollo de la aplicacion, especialmente para comprender conceptos nuevos, analizar alternativas de implementacion, identificar errores y acelerar la busqueda de soluciones.

La herramienta se utilizo de manera interactiva: se compartieron fragmentos de codigo, estructuras de clases, mensajes de error y resultados de pruebas para recibir explicaciones y propuestas que posteriormente se revisaron y adaptaron al proyecto.

## 2. Solicitudes realizadas en momentos importantes

El uso de ChatGPT estuvo presente durante distintas etapas del desarrollo.

### 2.1. Conversion de Minimal API a Web API

Se solicitaron explicaciones y ejemplos para transformar la estructura original del proyecto, desarrollado mediante Minimal API, a una arquitectura basada en controladores.

Las consultas se centraron en:

- Diferencias entre Minimal API y Web API con controladores.
- Organizacion de Controllers, Services, Interfaces y DTOs.
- Implementacion de inyeccion de dependencias.
- Separacion entre logica de negocio y controladores.
- Manejo de respuestas HTTP y codigos de estado.

ChatGPT proporciono ejemplos de implementacion y explicaciones sobre la responsabilidad de cada componente. Esto permitio comprender como organizar las operaciones existentes sin concentrar toda la logica dentro de los endpoints.

### 2.2. Entity Framework Core y MySQL

Se utilizo ChatGPT para comprender y resolver dudas relacionadas con la persistencia de datos.

Las solicitudes principales fueron:

- Configuracion de `AppDbContext`.
- Definicion de relaciones uno a uno y muchos a muchos.
- Configuracion de claves primarias y foraneas.
- Uso de Fluent API.
- Creacion y aplicacion de migraciones.
- Interpretacion de errores relacionados con Entity Framework Core.

La asistencia fue especialmente util para comprender como se relacionan las entidades y como se reflejan esas relaciones en las tablas de MySQL.

Tambien se compartieron configuraciones y errores para recibir explicaciones y corregir problemas durante la implementacion.

### 2.3. Desarrollo del Sistema de Misiones

Se solicito asistencia para estructurar y desarrollar el Sistema de Misiones, incorporado como funcionalidad propia del proyecto.

Las consultas incluyeron:

- Diseno de las entidades `Mision` y `ParticipaMision`.
- Relacion entre personajes y misiones.
- Organizacion de DTOs, interfaces y servicios.
- Validaciones para agregar participantes.
- Calculo de probabilidad de exito.
- Manejo de resultados y consecuencias sobre los personajes.
- Distribucion de recompensas.
- Uso de transacciones para mantener consistentes los cambios en la base de datos.

ChatGPT ayudo a analizar la logica de las operaciones y a identificar las condiciones necesarias para ejecutar una mision.

Las propuestas se utilizaron como apoyo para desarrollar la funcionalidad, pero las reglas de negocio fueron revisadas y ajustadas durante la implementacion.

### 2.4. Proteccion de datos

Se utilizaron consultas a ChatGPT para comprender e implementar los mecanismos de proteccion solicitados en el trabajo.

Los temas principales fueron:

- Diferencias entre hash y cifrado.
- Uso de `PasswordHasher` para proteger codigos de acceso.
- Funcionamiento de AES-GCM.
- Generacion de claves criptograficas.
- Almacenamiento de secretos mediante Secret Manager.
- Implementacion de interfaces y servicios de cifrado.
- Integracion de los mecanismos de proteccion con las credenciales de los personajes.

Tambien se solicitaron explicaciones sobre la diferencia entre almacenar un dato mediante hash y almacenarlo cifrado, ya que ambos mecanismos cumplen objetivos distintos.

La asistencia permitio comprender como implementar ambos mecanismos y como integrarlos en el servicio de credenciales.

### 2.5. Resolucion de errores y problemas de compilacion

Una de las aplicaciones mas frecuentes de ChatGPT durante el desarrollo fue la identificacion y correccion de errores tecnicos.

Cuando aparecian errores de compilacion, referencias incompatibles, problemas de configuracion o comportamientos inesperados, se compartian los mensajes de error y los fragmentos de codigo relacionados para analizar su posible causa.

La herramienta ayudo a:

- Interpretar mensajes de error del compilador.
- Identificar referencias o espacios de nombres incorrectos.
- Detectar problemas en configuraciones de servicios.
- Revisar errores relacionados con Entity Framework Core.
- Proponer modificaciones concretas para resolver problemas.
- Explicar por que ocurria un error y como evitarlo.

Este apoyo permitio reducir considerablemente el tiempo dedicado a investigar determinados problemas tecnicos. Algunos errores que, mediante una investigacion individual, podrian haber requerido bastante tiempo para localizar y comprender, pudieron resolverse mediante el analisis de los mensajes de error y la revision conjunta del codigo.

Las soluciones propuestas no se aplicaron automaticamente: se revisaron y se comprobo que fueran compatibles con la version y estructura del proyecto.

### 2.6. Configuracion y documentacion de Swagger

Se utilizo ChatGPT para configurar Swashbuckle y mejorar la documentacion de los endpoints.

Las solicitudes incluyeron:

- Instalacion y configuracion de Swagger.
- Definicion del titulo, version y descripcion de la API.
- Documentacion de operaciones mediante `EndpointSummary` y `EndpointDescription`.
- Descripcion de parametros y cuerpos de solicitud.
- Documentacion de respuestas HTTP.
- Analisis de problemas de comunicacion entre Swagger y la API.

Tambien se utilizo como referencia la documentacion del proyecto Minimal API original para mantener un estilo similar en la nueva Web API.

Durante esta etapa se presentaron problemas de compatibilidad con OpenAPI y de comunicacion entre Swagger y la API. Se compartieron capturas de pantalla y configuraciones para identificar las causas y adaptar las soluciones.

Finalmente, se opto por mantener descripciones funcionales y suficientemente detalladas, evitando agregar documentacion innecesaria.

## 3. Que se acepto, que se corrigio y por que

Las respuestas de ChatGPT se utilizaron como propuestas y orientacion. No todas se incorporaron directamente al proyecto.

Durante el desarrollo se aplicaron los siguientes criterios:

| Area                  | Sugerencia o aporte de IA                                    | Decision o correccion aplicada                                                                     |
| --------------------- | ------------------------------------------------------------ | -------------------------------------------------------------------------------------------------- |
| Arquitectura          | Organizacion mediante controladores, servicios e interfaces. | Se adapto a la estructura y funcionalidades existentes.                                            |
| Entity Framework Core | Ejemplos de relaciones y configuracion del contexto.         | Se revisaron las relaciones y se comprobaron mediante migraciones y consultas a MySQL.             |
| Sistema de Misiones   | Propuestas para estructurar la ejecucion y sus validaciones. | Se definieron y ajustaron las reglas de negocio del sistema.                                       |
| Proteccion de datos   | Ejemplos de hash, cifrado y almacenamiento de secretos.      | Se implementaron los mecanismos y se verifico su funcionamiento.                                   |
| Swagger               | Configuracion y descripciones de endpoints.                  | Se adapto la configuracion a las referencias disponibles y al estilo de documentacion deseado.     |
| Errores tecnicos      | Diagnostico y propuestas de correccion.                      | Se revisaron los cambios antes de aplicarlos y se comprobo que el proyecto compilara y funcionara. |

### Ejemplos concretos de correcciones

**Compatibilidad de OpenAPI:**

Durante la configuracion de Swagger se propuso utilizar `Microsoft.OpenApi.Models.OpenApiInfo`, pero el proyecto no reconocia ese espacio de nombres.

Se compartio el error de compilacion y se reviso la configuracion para utilizar la referencia compatible con las dependencias instaladas.

**Comunicacion de Swagger con la API:**

Swagger mostraba un error de red al intentar consultar un personaje. Se comprobo que el endpoint funcionaba al acceder directamente desde el navegador.

Despues de revisar los perfiles de ejecucion, se identifico que la prueba funcionaba correctamente utilizando HTTP. Se mantuvo esa configuracion para las pruebas locales.

**Documentacion de endpoints:**

Inicialmente se propuso documentar los controladores mediante comentarios XML. Sin embargo, se decidio utilizar `EndpointSummary` y `EndpointDescription` para mantener un estilo mas cercano al Swagger del proyecto original.

Estos ejemplos muestran que las sugerencias se revisaron y modificaron cuando no se ajustaban a las necesidades o configuraciones reales del proyecto.

## 4. Criterios de validacion

Durante el desarrollo se utilizaron diferentes mecanismos para comprobar las implementaciones y las soluciones propuestas:

- `dotnet build` para comprobar la compilacion del proyecto.
- Swagger para probar las operaciones HTTP.
- Consultas directas a MySQL para verificar los registros y las modificaciones realizadas.
- Inspeccion de las tablas, columnas, indices y relaciones.
- Pruebas de creacion y consulta de credenciales.
- Verificacion del almacenamiento de hashes y datos cifrados.
- Pruebas de ejecucion de eventos y misiones.

La utilizacion de ChatGPT no reemplazo estas comprobaciones. Las respuestas generadas se consideraron orientaciones que debian contrastarse con el comportamiento real de la aplicacion.

## 5. Responsabilidad del estudiante

ChatGPT fue utilizado como herramienta de apoyo para aprender, resolver problemas y avanzar en el desarrollo del proyecto.

El estudiante mantuvo la responsabilidad de revisar las propuestas, decidir cuales incorporar, adaptar el codigo a la estructura existente y comprobar los resultados.

El objetivo de utilizar la herramienta no fue solamente obtener fragmentos de codigo, sino comprender su funcionamiento y poder explicar las decisiones tecnicas y reglas de negocio implementadas.

## 6. Conclusion

La utilizacion de ChatGPT facilito el desarrollo del Catalogo Galactico, especialmente en la comprension de conceptos nuevos, la organizacion de la arquitectura, la implementacion de funcionalidades y la resolucion de errores tecnicos.

Tambien permitio reducir el tiempo dedicado a investigar determinados problemas de compilacion y configuracion, proporcionando explicaciones y propuestas de solucion que posteriormente fueron revisadas y probadas.

Se reconoce que las respuestas de inteligencia artificial pueden contener errores o sugerencias que no se ajusten al proyecto. Por ello, se considero necesario cuestionarlas, corregirlas y validar su funcionamiento antes de incorporarlas.

La responsabilidad sobre el codigo entregado, las decisiones de implementacion y la comprension del proyecto corresponde al estudiante.
