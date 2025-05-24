# To-DoAPI

## Descripción
To-Do es una Web API desarrollada en .NET 8 que permite gestionar tareas de manera flexible, utilizando un modelo genérico. Se pueden realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre tareas, garantizando validaciones y manejo adecuado de excepciones.

## Tecnologías utilizadas
- C#
- ASP.NET Core Web API
- .NET 8
- Programación asíncrona (async/await)

## Funcionalidades principales
- Crear nuevas tareas con diferentes tipos de datos asociados.
- Consultar todas las tareas o filtrar por criterios como estado o fecha.
- Actualizar detalles de las tareas existentes.
- Eliminar tareas innecesarias.
- Manejo global de excepciones y respuestas estructuradas.

## Cómo ejecutar el proyecto
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/snouel/To-Do-API-ITLA.git

2. Navegar a la carpeta del proyecto:
cd To-Do-API-ITLA/To-DoAPI

3.Restaurar los paquetes:
dotnet restore

4. Ejecutar la aplicación:
La API estará disponible en https://localhost:5001 o http://localhost:5000 (según tu configuración).

## Etapa 2: Delegados, Funciones Anónimas, Action y Func

En esta etapa se integraron mecanismos que hacen la lógica de negocio más flexible y reutilizable mediante el uso de:

- **Delegado personalizado `ValidateTask`** para validar tareas antes de crearlas (verifica que la descripción no esté vacía y la fecha de vencimiento sea válida).
- **`Action<TodoTask<string>>`** para notificar automáticamente en consola cada vez que se crea una nueva tarea.
- **`Func<TodoTask<string>, int>`** para calcular y mostrar los días restantes hasta la fecha de vencimiento.
- **Funciones anónimas con LINQ** para filtrar dinámicamente tareas pendientes desde el controlador.

Estas implementaciones se integraron directamente en el `TasksController`, haciendo la lógica más limpia, reutilizable y extensible sin modificar métodos existentes.
La API estará disponible en https://localhost:5001 o http://localhost:5000 (según tu configuración).

## Etapa 3: Patrones de Diseño - Fábrica (Factory)

En esta etapa se implementó el patrón de diseño Factory para centralizar la creación de tareas con configuraciones predefinidas. Esto permite mantener la lógica de construcción de objetos en una sola clase reutilizable.

Cambios realizados:
Se creó la clase TasksFactory en la carpeta Factories/.

Se implementaron los métodos:

CreateHighPriorityTask: crea una tarea con vencimiento de 1 día y prioridad alta.

CreateLowPriorityTask: crea una tarea con vencimiento de 7 días y prioridad baja.

CreateCustomTask: permite construir una tarea con parámetros personalizados.

Se crearon los endpoints POST /api/tasks/customizable-task, /api/tasks/high-priority, /api/tasks/low-priority  que usa la fábrica para construir tareas de diferente tipos.

Esta implementación mejora la modularidad y escalabilidad de la API, permitiendo agregar fácilmente nuevos tipos de tareas sin duplicar lógica en el controlador.

##  Etapa 4: Programación Reactiva con Rx.NET y Cola Secuencial

En esta etapa se implementó una **cola FIFO** utilizando `Queue<T>` y `Rx.NET` para procesar tareas de forma secuencial, asegurando que **solo una tarea se procese a la vez** y que la siguiente espere hasta que la anterior esté completada.

###  Cambios realizados:

- Se creó la clase `TaskQueueHandler` como `Singleton`.
- Se implementó una cola (`ConcurrentQueue<TodoTask<string>>`) para almacenar tareas en espera.
- Se utilizó `Observable.Start()` de Rx.NET para simular el procesamiento asíncrono de cada tarea.
- El procesamiento de cada tarea incluye:
  - Estado inicial `"Pending"`
  - Simulación de ejecución con `await Task.Delay(...)`
  - Estado final `"Completed"`
- Las tareas se procesan en el mismo orden en que fueron agregadas (First In, First Out).

### Consideraciones técnicas:

- `TaskQueueHandler` se registró como `Singleton` para mantener una cola global.
- El repositorio (`TaskRepository`) también se registró como `Singleton` para conservar el estado de las tareas en memoria entre requests.
- El servicio (`TaskService`) se mantiene como `Scoped`.

### Cómo probarlo:

1. Realiza múltiples llamadas `POST` para crear tareas.
2. Cada tarea será encolada y procesada una por una.
3. En consola se mostrará:

	Procesando tarea: Tarea 1
	Completada: Tarea 1
	Procesando tarea: Tarea 2
	Completada: Tarea 2
