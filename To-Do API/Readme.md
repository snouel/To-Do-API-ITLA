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
