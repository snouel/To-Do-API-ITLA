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
````

2. Navegar a la carpeta del proyecto:

```bash
cd To-Do-API-ITLA/To-DoAPI
```

3. Restaurar los paquetes:

```bash
dotnet restore
```

4. Ejecutar la aplicación:
   La API estará disponible en [https://localhost:5001](https://localhost:5001) o [http://localhost:5000](http://localhost:5000) (según tu configuración).

---

## Etapa 2: Delegados, Funciones Anónimas, Action y Func

En esta etapa se integraron mecanismos que hacen la lógica de negocio más flexible y reutilizable mediante el uso de:

* **Delegado personalizado `ValidateTask`** para validar tareas antes de crearlas (verifica que la descripción no esté vacía y la fecha de vencimiento sea válida).
* **`Action<TodoTask<string>>`** para notificar automáticamente en consola cada vez que se crea una nueva tarea.
* **`Func<TodoTask<string>, int>`** para calcular y mostrar los días restantes hasta la fecha de vencimiento.
* **Funciones anónimas con LINQ** para filtrar dinámicamente tareas pendientes desde el controlador.

Estas implementaciones se integraron directamente en el `TasksController`, haciendo la lógica más limpia, reutilizable y extensible sin modificar métodos existentes.

---

## Etapa 3: Patrones de Diseño - Fábrica (Factory)

En esta etapa se implementó el patrón de diseño Factory para centralizar la creación de tareas con configuraciones predefinidas. Esto permite mantener la lógica de construcción de objetos en una sola clase reutilizable.

**Cambios realizados:**

* Se creó la clase `TasksFactory` en la carpeta `Factories/`.

* Se implementaron los métodos:

  * `CreateHighPriorityTask`: crea una tarea con vencimiento de 1 día y prioridad alta.
  * `CreateLowPriorityTask`: crea una tarea con vencimiento de 7 días y prioridad baja.
  * `CreateCustomTask`: permite construir una tarea con parámetros personalizados.

* Se crearon los endpoints:

  * `POST /api/tasks/customizable-task`
  * `POST /api/tasks/high-priority`
  * `POST /api/tasks/low-priority`

Esta implementación mejora la modularidad y escalabilidad de la API, permitiendo agregar fácilmente nuevos tipos de tareas sin duplicar lógica en el controlador.

---

## Etapa 4: Programación Reactiva con Rx.NET y Cola Secuencial

Se implementó una **cola FIFO** utilizando `Queue<T>` y `Rx.NET` para procesar tareas de forma secuencial, asegurando que **solo una tarea se procese a la vez** y que la siguiente espere hasta que la anterior esté completada.

### Cambios realizados:

* Se creó la clase `TaskQueueHandler` como `Singleton`.
* Se implementó una cola (`ConcurrentQueue<TodoTask<string>>`) para almacenar tareas en espera.
* Se utilizó `Observable.Start()` de Rx.NET para simular el procesamiento asíncrono de cada tarea.
* El procesamiento de cada tarea incluye:

  * Estado inicial `"Pending"`
  * Simulación de ejecución con `await Task.Delay(...)`
  * Estado final `"Completed"`

### Consideraciones técnicas:

* `TaskQueueHandler` se registró como `Singleton` para mantener una cola global.
* El repositorio (`TaskRepository`) también se registró como `Singleton` para conservar el estado de las tareas en memoria entre requests.
* El servicio (`TaskService`) se mantiene como `Scoped`.

### Cómo probarlo:

1. Realiza múltiples llamadas `POST` para crear tareas.
2. Cada tarea será encolada y procesada una por una.
3. En consola se mostrará:

```
Procesando tarea: Tarea 1
Completada: Tarea 1
Procesando tarea: Tarea 2
Completada: Tarea 2
```

---

## Etapa 5 - Optimización con Memoization

En esta etapa se implementó la técnica de memoization (memorización) para optimizar funciones repetitivas y costosas dentro de la API, con el objetivo de mejorar el rendimiento general y evitar cálculos innecesarios.

**Funcionalidad implementada:**

* Se creó una función pura para calcular el porcentaje de tareas completadas.
* Se utilizó un `Dictionary<string, double>` como caché para almacenar los resultados de esos cálculos.
* Se encapsuló esta lógica en una clase estática `MemoizationHelper`.
* Se generó una clave única (key) para cada conjunto de entradas, y si ya existía en caché, se devolvió el valor almacenado.

**Archivos clave:**

* `Helpers/MemoizationHelper.cs`: gestiona la caché de resultados.
* `Controller/TaskController.cs`: utiliza memoization con `MemoizationHelper.CalculateCompletionPercentage(completedTasks, totalTasks);`

---

## Etapa 6 – Autenticación y Autorización con JWT

**Objetivo:**
Implementar un sistema de autenticación basado en JSON Web Tokens (JWT) para controlar el acceso a los recursos de la API.

**Cambios implementados:**

1. **Entidades y DTOs**

   * `User`, `LoginUserDTO`, `RegisterUserDTO`.

2. **Repositorio de usuarios**

   * Interfaz `IUserRepository`.
   * Implementación en memoria con `ConcurrentDictionary`.

3. **Servicio de autenticación (`AuthService`)**

   * `RegisterAsync`: valida y registra nuevos usuarios.
   * `AuthenticateAsync`: valida credenciales y genera JWT.

4. **JWT**

   * Contiene `email`, `jti`.
   * Firmado con HMAC-SHA256.

5. **Controlador `AuthController`**

   * `POST /api/auth/register`
   * `POST /api/auth/login`

6. **Seguridad en la API**

   * `AddAuthentication` y `AddAuthorization`
   * Endpoints protegidos con `[Authorize]`
   * Validación automática de tokens en los requests

**Resultado:**
La API ahora está protegida con un sistema JWT funcional que:

* Genera tokens únicos por sesión.
* Controla el acceso a endpoints.
* No requiere almacenamiento de sesión.

---

## Etapa 7 – Configuración de SignalR

**Objetivo:**
Establecer infraestructura para comunicación en tiempo real mediante SignalR.

**Cambios implementados:**

1. Se agregó `Microsoft.AspNetCore.SignalR`.
2. Se registró SignalR con `builder.Services.AddSignalR()`.
3. Se creó el `TasksHub`.
4. Se notifican los clientes con:

```csharp
await _hubContext.Clients.All.SendAsync("TaskCreated", newTask);
```

5. Se desarrolló un cliente de consola para pruebas (no productivo).

**Flujo de comunicación:**

* El cliente se conecta al `/taskHub`.
* Cuando se crea una tarea, el servidor emite `TaskCreated`.
* Los clientes reciben el evento automáticamente.

**Resultado esperado:**

* Comunicación en tiempo real sin polling.
* Estructura lista para eventos como `TaskUpdated`, `TaskDeleted`.

---

## Etapa 8 – Pruebas Unitarias con xUnit

Se implementaron pruebas unitarias para validar funcionalidades críticas usando xUnit y Moq.

### Objetivos

* Validar servicios de autenticación, tareas y cola.
* Verificar controladores.
* Evaluar funciones puras.
* Simular datos sin base de datos real.

### Cobertura de pruebas

| Archivo                        | Casos principales                                     |
| ------------------------------ | ----------------------------------------------------- |
| AuthServicesTests.cs           | Inicio de sesión, credenciales inválidas, registro    |
| TaskServiceTests.cs            | Creación y actualización de tareas                    |
| TaskQueueHanlderServiceTest.cs | Encolado y procesamiento de tareas en background      |
| MemoizationHelperTests.cs      | Cálculo y cacheo del porcentaje de tareas completadas |
| TasksControllersTests.cs       | Obtener tarea por ID desde el controlador             |

### Herramientas utilizadas

* xUnit
* Moq
* .NET 6 o superior

