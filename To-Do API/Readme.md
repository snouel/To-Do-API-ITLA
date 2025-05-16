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