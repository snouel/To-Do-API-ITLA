# To-DoAPI

## Descripci�n
To-Do es una Web API desarrollada en .NET 8 que permite gestionar tareas de manera flexible, utilizando un modelo gen�rico. Se pueden realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre tareas, garantizando validaciones y manejo adecuado de excepciones.

## Tecnolog�as utilizadas
- C#
- ASP.NET Core Web API
- .NET 8
- Programaci�n as�ncrona (async/await)

## Funcionalidades principales
- Crear nuevas tareas con diferentes tipos de datos asociados.
- Consultar todas las tareas o filtrar por criterios como estado o fecha.
- Actualizar detalles de las tareas existentes.
- Eliminar tareas innecesarias.
- Manejo global de excepciones y respuestas estructuradas.

## C�mo ejecutar el proyecto
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/snouel/To-Do-API-ITLA.git

2. Navegar a la carpeta del proyecto:
cd To-Do-API-ITLA/To-DoAPI

3.Restaurar los paquetes:
dotnet restore

4. Ejecutar la aplicaci�n:
La API estar� disponible en https://localhost:5001 o http://localhost:5000 (seg�n tu configuraci�n).

## Etapa 2: Delegados, Funciones An�nimas, Action y Func

En esta etapa se integraron mecanismos que hacen la l�gica de negocio m�s flexible y reutilizable mediante el uso de:

- **Delegado personalizado `ValidateTask`** para validar tareas antes de crearlas (verifica que la descripci�n no est� vac�a y la fecha de vencimiento sea v�lida).
- **`Action<TodoTask<string>>`** para notificar autom�ticamente en consola cada vez que se crea una nueva tarea.
- **`Func<TodoTask<string>, int>`** para calcular y mostrar los d�as restantes hasta la fecha de vencimiento.
- **Funciones an�nimas con LINQ** para filtrar din�micamente tareas pendientes desde el controlador.

Estas implementaciones se integraron directamente en el `TasksController`, haciendo la l�gica m�s limpia, reutilizable y extensible sin modificar m�todos existentes.
La API estar� disponible en https://localhost:5001 o http://localhost:5000 (seg�n tu configuraci�n).

## Etapa 3: Patrones de Dise�o - F�brica (Factory)

En esta etapa se implement� el patr�n de dise�o Factory para centralizar la creaci�n de tareas con configuraciones predefinidas. Esto permite mantener la l�gica de construcci�n de objetos en una sola clase reutilizable.

Cambios realizados:
Se cre� la clase TasksFactory en la carpeta Factories/.

Se implementaron los m�todos:

CreateHighPriorityTask: crea una tarea con vencimiento de 1 d�a y prioridad alta.

CreateLowPriorityTask: crea una tarea con vencimiento de 7 d�as y prioridad baja.

CreateCustomTask: permite construir una tarea con par�metros personalizados.

Se crearon los endpoints POST /api/tasks/customizable-task, /api/tasks/high-priority, /api/tasks/low-priority  que usa la f�brica para construir tareas de diferente tipos.

Esta implementaci�n mejora la modularidad y escalabilidad de la API, permitiendo agregar f�cilmente nuevos tipos de tareas sin duplicar l�gica en el controlador.

##  Etapa 4: Programaci�n Reactiva con Rx.NET y Cola Secuencial

En esta etapa se implement� una **cola FIFO** utilizando `Queue<T>` y `Rx.NET` para procesar tareas de forma secuencial, asegurando que **solo una tarea se procese a la vez** y que la siguiente espere hasta que la anterior est� completada.

###  Cambios realizados:

- Se cre� la clase `TaskQueueHandler` como `Singleton`.
- Se implement� una cola (`ConcurrentQueue<TodoTask<string>>`) para almacenar tareas en espera.
- Se utiliz� `Observable.Start()` de Rx.NET para simular el procesamiento as�ncrono de cada tarea.
- El procesamiento de cada tarea incluye:
  - Estado inicial `"Pending"`
  - Simulaci�n de ejecuci�n con `await Task.Delay(...)`
  - Estado final `"Completed"`
- Las tareas se procesan en el mismo orden en que fueron agregadas (First In, First Out).

### Consideraciones t�cnicas:

- `TaskQueueHandler` se registr� como `Singleton` para mantener una cola global.
- El repositorio (`TaskRepository`) tambi�n se registr� como `Singleton` para conservar el estado de las tareas en memoria entre requests.
- El servicio (`TaskService`) se mantiene como `Scoped`.

### C�mo probarlo:

1. Realiza m�ltiples llamadas `POST` para crear tareas.
2. Cada tarea ser� encolada y procesada una por una.
3. En consola se mostrar�:

	Procesando tarea: Tarea 1
	Completada: Tarea 1
	Procesando tarea: Tarea 2
	Completada: Tarea 2

##   Etapa 5 - Optimizaci�n con Memoization

En esta etapa se implement� la t�cnica de memoization (memorizaci�n) para optimizar funciones repetitivas y costosas dentro de la API, con el objetivo de mejorar el rendimiento general y evitar c�lculos innecesarios.

Funcionalidad implementada:

	- Se cre� una funci�n pura para calcular el porcentaje de tareas completadas.

	- Se utiliz� un Dictionary<string, double> como cach� para almacenar los resultados de esos c�lculos.

	- Se encapsul� esta l�gica en una clase est�tica MemoizationHelper.

	- Se gener� una clave �nica (key) para cada conjunto de entradas, y si ya exist�a en cach�, se devolvi� el valor almacenado.


Archivos clave: 

	- Helpers/MemoizationHelper.cs: gestiona la cach� de resultados.

	- Controller/TaskController.cs: utiliza memoization con MemoizationHelper.CalculateCompletionPercentage(completedTasks, totalTasks);


## Etapa 6 � Autenticaci�n y Autorizaci�n con JWT

Objetivo

Implementar un sistema de autenticaci�n basado en JSON Web Tokens (JWT) para controlar el acceso a los recursos de la API. Este mecanismo permite validar la identidad del usuario y aplicar control de acceso por roles, sin almacenar sesiones en el servidor.

Cambios implementados: 

	-  1. Entidades y DTOs

		User: nueva entidad con Id, Username, Password, Role, CreatedAt, UpdatedAt.

		LogUserDTO: para login (email y password).

		RegisterUserDTO: para registrar usuarios.

	- 2. Repositorio de usuarios

		IUserRepository: interfaz para gestionar usuarios.

		IUserRepository: implementaci�n en memoria (ConcurrentDictionary con ID autoincrementable).

	- 3. Servicio de autenticaci�n (AuthService)

		M�todo RegisterAsync: valida y registra nuevos usuarios.

		M�todo AuthenticateAsync: valida credenciales, genera y firma un JWT.

		El JWT incluye: email y Jti (ID �nico por token).

		Firma con clave secreta usando HMAC-SHA256.

	4. Controlador AuthController

		POST /api/auth/register: permite registrar un nuevo usuario.

		POST /api/auth/login: valida credenciales y retorna un token JWT.

	5. Seguridad en la API
		Middleware configurado con AddAuthentication y AddAuthorization.

	Tokens protegidos mediante Authorization: Bearer <token>.

	Endpoints protegidos con [Authorize].

Flujo de autenticaci�n

	El cliente se registra con /api/auth/register.

	Luego inicia sesi�n con /api/auth/login y recibe un token.

	El cliente env�a el token en cada request (Authorization: Bearer).

	La API valida el token autom�ticamente antes de procesar la solicitud.

## Resultado
La API ahora est� protegida con un sistema JWT funcional que:

Genera tokens �nicos por sesi�n.

Controla el acceso a endpoints por autenticaci�n y rol.

No requiere almacenamiento de sesi�n en el servidor.

 ## Etapa 7 – Configuración de SignalR

	Objetivo

		Establecer la infraestructura base para comunicación en tiempo real mediante SignalR, permitiendo que el servidor notifique automáticamente a todos los clientes conectados cuando se cree una nueva tarea, sin necesidad de que los clientes realicen peticiones repetitivas o recarguen la página.

	Cambios implementados
	
		1. Instalación y configuración de SignalR
		Se agregó el paquete Microsoft.AspNetCore.SignalR.

		Se registró el servicio con builder.Services.AddSignalR().

		2. Creación del Hub
		Se creó la clase TasksHub, heredando de Hub.

		Se configuró el endpoint /taskHub en Program.cs con:

		app.MapHub<TasksHub>("/taskHub");
		
		3. Emisión del evento al crear una tarea
		En el servicio TaskService, luego de registrar la nueva tarea, se emite el evento:

		await _hubContext.Clients.All.SendAsync("TaskCreated", newTask);
		Esto notifica a todos los clientes conectados que una nueva tarea fue creada.

		4. Cliente de prueba
		Se desarrolló un cliente SignalR (HTML o consola .NET) que se conecta a /taskHub y escucha el evento TaskCreated.

		El cliente ejecuta una acción automática cuando recibe la notificación, como mostrar la tarea por consola.

	Flujo de comunicación

		El cliente se conecta al hub /taskHub usando SignalR.

		Cuando se llama al endpoint POST /api/tasks, el servidor:

		Crea la nueva tarea.

		Notifica a todos los clientes vía Clients.All.SendAsync("TaskCreated", response).

		Los clientes reciben el evento TaskCreated con la tarea nueva y reaccionan en tiempo real.

	Resultado esperado

		Los clientes conectados son notificados instantáneamente de nuevas tareas.

		No necesitan hacer polling ni refrescar la vista.

		La estructura está lista para extenderse con otros eventos (TaskUpdated, TaskDeleted, etc.).






