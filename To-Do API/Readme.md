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