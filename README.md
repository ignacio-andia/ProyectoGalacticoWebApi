# Catalogo Galactico - API REST

API REST desarrollada con ASP.NET Core Web API para administrar personajes, cards, eventos y misiones del universo Star Wars.

El proyecto permite gestionar la informacion de los personajes, sus caracteristicas, participaciones en eventos y misiones, asi como sus credenciales de acceso. Utiliza MySQL como sistema de almacenamiento y Entity Framework Core para la persistencia de datos.

---

## 1. Funcionalidades principales

### 1.1. Gestion de personajes

Permite administrar los personajes registrados en el catalogo.

Operaciones disponibles:

- Registrar personajes.
- Consultar todos los personajes.
- Consultar un personaje por su identificador.
- Actualizar la informacion de un personaje.
- Eliminar personajes.
- Modificar el estado de un personaje.

Los personajes cuentan con informacion como nombre, especie, faccion, afiliacion, estado, sensibilidad a la Fuerza, imagen y monedas.

### 1.2. Gestion de Cards

Cada personaje puede tener una card asociada con sus caracteristicas de combate.

La card contiene:

- Poder.
- Habilidad especial.
- Arma.
- Nivel de peligrosidad.

Operaciones disponibles:

- Registrar una card.
- Consultar todas las cards.
- Consultar una card por su identificador.
- Consultar la card de un personaje.
- Actualizar una card.
- Eliminar una card.

### 1.3. Gestion de eventos

Permite organizar eventos y administrar los personajes que participan en ellos.

Operaciones disponibles:

- Crear eventos.
- Consultar eventos.
- Consultar un evento por su identificador.
- Actualizar eventos.
- Agregar participantes.
- Retirar participantes.
- Consultar los participantes de un evento.
- Consultar los eventos de un personaje.
- Obtener el MVP de un evento.
- Simular enfrentamientos entre facciones.

La simulacion considera el poder de los participantes, sus facciones y un factor aleatorio que influye en el resultado.

Los personajes de la faccion perdedora mueren, mientras que los personajes neutrales no mueren como consecuencia del enfrentamiento.

### 1.4. Sistema de misiones

Sistema propio que permite crear y ejecutar misiones mediante la participacion de personajes.

Cada mision contiene:

- Nombre.
- Tipo.
- Nivel de dificultad.
- Descripcion.
- Poder minimo requerido.
- Recompensa en monedas.
- Estado.
- Resultado.

Operaciones disponibles:

- Crear misiones.
- Consultar todas las misiones.
- Agregar personajes como participantes.
- Ejecutar misiones.

Reglas principales:

- Una mision debe estar disponible para recibir participantes.
- Los personajes deben estar vivos.
- Los personajes deben tener una card registrada.
- Todos los participantes deben pertenecer a la misma faccion.
- Un personaje no puede participar simultaneamente en varias misiones disponibles.
- El poder total de los participantes debe alcanzar el minimo requerido.
- Una mision solamente puede ejecutarse una vez.

La probabilidad de exito se calcula considerando el poder total de los participantes y el nivel de dificultad de la mision. El resultado se determina aleatoriamente y la probabilidad se limita a un rango entre 5% y 95%.

Consecuencias de la ejecucion:

- Mision exitosa: cada participante puede terminar vivo o herido.
- Mision fallida: todos los participantes mueren.
- La recompensa se distribuye entre los participantes vivos o heridos.
- Los personajes muertos no reciben monedas.

La ejecucion utiliza una transaccion de base de datos para mantener consistentes los cambios realizados durante la mision.

### 1.5. Gestion de credenciales

Permite registrar y consultar credenciales asociadas a los personajes.

Las credenciales contienen:

- Codigo de acceso.
- Canal de comunicacion.

Para proteger la informacion se utilizan dos mecanismos:

- El codigo de acceso se almacena mediante hash utilizando `PasswordHasher`.
- El canal de comunicacion se almacena cifrado mediante AES-GCM.

Al consultar las credenciales, el canal de comunicacion se descifra antes de devolverlo mediante la API.

El hash del codigo de acceso nunca se incluye en las respuestas de la API.

---

## 2. Tecnologias utilizadas

| Tecnologia                       | Uso                                                    |
| -------------------------------- | ------------------------------------------------------ |
| C#                               | Lenguaje de programacion                               |
| .NET 10                          | Plataforma de desarrollo                               |
| ASP.NET Core Web API             | Desarrollo de la API REST                              |
| Entity Framework Core            | ORM y persistencia de datos                            |
| Pomelo.EntityFrameworkCore.MySql | Proveedor de MySQL para EF Core                        |
| MySQL 8.0.36                     | Sistema gestor de base de datos                        |
| Swashbuckle.AspNetCore           | Generacion de documentacion Swagger                    |
| AES-GCM                          | Cifrado de informacion sensible                        |
| PasswordHasher                   | Hash de codigos de acceso                              |
| Secret Manager                   | Almacenamiento local de secretos durante el desarrollo |

---

## 3. Arquitectura del proyecto

El proyecto utiliza una arquitectura organizada por responsabilidades.

```text
ProyectoGalacticoWebApi/
│
├── Controllers/
│   ├── PersonajeController.cs
│   ├── CardsPersonajeController.cs
│   ├── EventoController.cs
│   ├── MisionController.cs
│   └── CredencialController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Dtos/
│
├── Migrations/
│
├── Models/
│
├── Results/
│   └── ResultadoOperacion.cs
│
├── Services/
│   ├── Interfaces/
│   ├── PersonajeService.cs
│   ├── CardsPersonajeService.cs
│   ├── EventoService.cs
│   ├── MisionService.cs
│   ├── CredencialService.cs
│   └── CifradoService.cs
│
├── Properties/
│
├── Program.cs
├── appsettings.json
├── .gitignore
└── README.md
```

### Responsabilidades

**Controllers:** reciben las solicitudes HTTP, llaman a los servicios correspondientes y devuelven las respuestas HTTP.

**Services:** contienen la logica de negocio y las operaciones relacionadas con cada modulo.

**Interfaces:** definen los contratos de los servicios y permiten aplicar inyeccion de dependencias.

**DTOs:** definen los datos que reciben y devuelven los endpoints, separando las entidades de las solicitudes HTTP.

**Models:** representan las entidades utilizadas por el sistema.

**Data:** contiene `AppDbContext`, encargado de configurar las entidades y relaciones de Entity Framework Core.

**Migrations:** almacenan los cambios realizados sobre el esquema de la base de datos.

**Results:** contiene `ResultadoOperacion<T>`, utilizado para comunicar resultados y errores entre los servicios y los controladores.

---

## 4. Modelo de datos

El sistema cuenta con las siguientes entidades principales:

| Entidad             | Descripcion                                           |
| ------------------- | ----------------------------------------------------- |
| Personaje           | Almacena la informacion de cada personaje.            |
| CardsPersonaje      | Contiene las caracteristicas y atributos de combate.  |
| Evento              | Representa los eventos del catalogo.                  |
| ParticipaEvento     | Relaciona personajes con eventos.                     |
| Mision              | Almacena las misiones disponibles y sus resultados.   |
| ParticipaMision     | Relaciona personajes con misiones.                    |
| CredencialPersonaje | Almacena las credenciales asociadas a cada personaje. |

### Relaciones

- Personaje - CardsPersonaje: relacion uno a uno (1:1).
- Personaje - CredencialPersonaje: relacion uno a uno (1:1).
- Personaje - Evento: relacion muchos a muchos (N) mediante ParticipaEvento.
- Personaje - Mision: relacion muchos a muchos (N) mediante ParticipaMision.

Las relaciones y restricciones se configuran mediante `AppDbContext`.

---

## 5. Requisitos previos

Para ejecutar el proyecto se necesita:

- Windows, Linux o macOS.
- .NET SDK 10.
- MySQL Server 8.0 o compatible.
- MySQL Workbench u otra herramienta para administrar MySQL.
- Git.
- Un editor compatible con C#, como Visual Studio o Visual Studio Code.

Tambien se necesita el comando `dotnet ef` para administrar las migraciones de Entity Framework Core.

Si no esta instalado, se puede instalar mediante:

```bash
dotnet tool install --global dotnet-ef
```

---

## 6. Instalacion y configuracion

### 6.1. Clonar el repositorio

Clonar el repositorio desde GitHub:

```bash
git clone <URL_DEL_REPOSITORIO>
```

Ingresar a la carpeta del proyecto:

```bash
cd ProyectoGalacticoWebApi
```

### 6.2. Restaurar dependencias

Ejecutar:

```bash
dotnet restore
```

Esto descarga las dependencias necesarias definidas en el archivo `.csproj`.

### 6.3. Crear la base de datos y el usuario MySQL

Antes de iniciar la API, se debe crear la base de datos y el usuario que utilizara la aplicacion.

Ejecutar el siguiente script desde MySQL Workbench:

```sql
CREATE DATABASE catalogo_galactico_db CHARACTER SET utf8mb4;

CREATE USER 'galactico_user'@'localhost'
IDENTIFIED BY '<CONTRASENA_SEGURA>';

GRANT ALL PRIVILEGES ON catalogo_galactico_db.*
TO 'galactico_user'@'localhost';

FLUSH PRIVILEGES;
```

Reemplazar `<CONTRASENA_SEGURA>` por una contrasena propia.

Este usuario tendra permisos sobre la base de datos `catalogo_galactico_db`.

**Importante:** no utilizar la contrasena del ejemplo ni guardar credenciales reales dentro del repositorio.

### 6.4. Configurar Secret Manager

El proyecto utiliza Secret Manager para almacenar informacion sensible fuera de los archivos versionados.

Se deben configurar dos valores:

- `ConnectionStrings:DefaultConnection`
- `Cifrado:Clave`

Primero, ingresar a la carpeta donde se encuentra el archivo `.csproj`.

Si el proyecto no tiene inicializado User Secrets, ejecutar:

```bash
dotnet user-secrets init
```

Configurar la cadena de conexion:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=catalogo_galactico_db;User=galactico_user;Password=<CONTRASENA_SEGURA>;"
```

Reemplazar `<CONTRASENA_SEGURA>` por la misma contrasena utilizada al crear el usuario MySQL.

#### Generar la clave de cifrado

El servicio AES-GCM necesita una clave de 256 bits, equivalente a 32 bytes.

En PowerShell se puede generar una clave aleatoria mediante:

```powershell
$bytes = New-Object byte[] 32
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
$rng.GetBytes($bytes)
$clave = [Convert]::ToBase64String($bytes)
$rng.Dispose()
```

Guardar la clave generada en Secret Manager: (se aplica en la carpeta del proyecto)

```powershell
dotnet user-secrets set "Cifrado:Clave" $clave
```

La clave generada debe mantenerse privada y no debe incluirse en el repositorio.

**Importante:** si se pierde o cambia la clave despues de cifrar informacion, los datos existentes no podran descifrarse con la nueva clave.

Para comprobar que los secretos fueron configurados:

```bash
dotnet user-secrets list
```

No compartir la salida de este comando, ya que contiene los valores almacenados.

---

## 7. Migraciones de Entity Framework Core

El proyecto utiliza migraciones para crear y evolucionar el esquema de la base de datos.

Las migraciones incluidas son:

| Migracion                     | Descripcion                                                       |
| ----------------------------- | ----------------------------------------------------------------- |
| Inicial                       | Creacion inicial de las entidades principales.                    |
| PermitirResultadoNulo         | Permite resultados nulos en los eventos.                          |
| AgregarSistemaMisiones        | Incorpora el sistema de misiones y las monedas de los personajes. |
| AgregarCredencialesPersonajes | Incorpora las credenciales y su relacion con personajes.          |

Para aplicar todas las migraciones pendientes y actualizar la base de datos:

```bash
dotnet ef database update
```

Este comando utiliza la cadena de conexion configurada en Secret Manager.

### Comandos utiles

Consultar las migraciones existentes:

```bash
dotnet ef migrations list
```

Crear una nueva migracion despues de modificar los modelos:

```bash
dotnet ef migrations add NombreDeLaMigracion
```

Aplicar los cambios a la base de datos:

```bash
dotnet ef database update
```

Las migraciones existentes forman parte del repositorio y no necesitan generarse nuevamente al clonar el proyecto.

---

## 8. Ejecucion del proyecto

Una vez configurada la base de datos y los secretos, ingresar a la carpeta del proyecto y ejecutar:

```bash
dotnet build
```

Si la compilacion termina correctamente, iniciar la API:

```bash
dotnet run
```

Para iniciar utilizando el perfil HTTP configurado en el proyecto:

```bash
dotnet run --launch-profile http
```

La API quedara disponible en la direccion configurada en `Properties/launchSettings.json`.

### Acceso a Swagger

Con el perfil HTTP configurado actualmente:

```text
http://localhost:5126/swagger
```

Desde Swagger se pueden consultar y probar los endpoints disponibles. (Puede cambiar el puerto)

---

## 9. Documentacion de endpoints

La API cuenta con documentacion generada mediante Swagger.

Los controladores estan organizados por modulo:

| Controlador              | Ruta base             |
| ------------------------ | --------------------- |
| PersonajeController      | `/api/Personaje`      |
| CardsPersonajeController | `/api/CardsPersonaje` |
| EventoController         | `/api/Evento`         |
| MisionController         | `/api/Mision`         |
| CredencialController     | `/api/Credencial`     |

Cada operacion documenta su funcionamiento, parametros, requisitos y posibles respuestas HTTP.

---

## 10. Seguridad y proteccion de datos

El proyecto implementa mecanismos de proteccion para la informacion sensible.

### Hash de codigos de acceso

Los codigos de acceso de las credenciales se procesan mediante `PasswordHasher`.

El valor original no se almacena directamente en MySQL. En su lugar, se guarda el resultado del proceso de hash.

El hash tampoco se devuelve mediante los DTOs de respuesta.

### Cifrado de informacion sensible

El canal de comunicacion de las credenciales se cifra mediante AES-GCM.

La implementacion utiliza:

- Clave de 256 bits.
- Nonce aleatorio de 12 bytes.
- Tag de autenticacion de 16 bytes.
- Almacenamiento del resultado codificado en Base64.

Al consultar las credenciales, el servicio descifra el canal antes de devolverlo mediante la API.

### Gestion de secretos

La cadena de conexion y la clave de cifrado se almacenan mediante Secret Manager durante el desarrollo.

Estos valores no deben guardarse en archivos versionados ni publicarse en GitHub.

### Limitaciones de seguridad

El proyecto no implementa autenticacion de usuarios ni autorizacion mediante JWT.

Por lo tanto, los endpoints no cuentan con un mecanismo de autenticacion para controlar quien puede realizar las operaciones. La proteccion implementada se concentra en el almacenamiento de los datos sensibles.

---

## 11. Componente propio: Sistema de Misiones

El Sistema de Misiones constituye una funcionalidad propia desarrollada para ampliar el catalogo original.

Permite crear misiones, asignar personajes y ejecutar operaciones con resultados aleatorios condicionados por el poder de los participantes y la dificultad.

El sistema administra:

- Estados de las misiones.
- Participantes.
- Requisitos de poder.
- Probabilidad de exito.
- Resultados de ejecucion.
- Consecuencias sobre los personajes.
- Distribucion de recompensas.

La ejecucion de una mision actualiza los estados de los personajes y sus monedas dentro de una transaccion de base de datos.

---

## 12. Consideraciones para el desarrollo

- No almacenar contrasenas ni claves de cifrado dentro del codigo fuente.
- No subir archivos locales de configuracion con secretos.
- Mantener las migraciones dentro del repositorio.
- Ejecutar `dotnet ef database update` despues de clonar el proyecto y configurar la conexion.
- Verificar que MySQL este iniciado antes de ejecutar la API.
- Mantener la clave de cifrado original para poder descifrar los datos almacenados.
- Realizar commits descriptivos al incorporar cambios.

---

## 13. Autor

Proyecto academico desarrollado para la asignatura Tecnologias Web 1.

**Proyecto:** Catalogo Galactico - API REST.

**Tecnologias principales:** ASP.NET Core Web API, Entity Framework Core y MySQL.
