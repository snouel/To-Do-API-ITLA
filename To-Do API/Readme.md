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
