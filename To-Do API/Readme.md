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
