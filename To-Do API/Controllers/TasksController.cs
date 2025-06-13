using Microsoft.AspNetCore.Mvc;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Delegates;
using To_Do_API.Application.Factories;
using To_Do_API.Application.Helpers;
using To_Do_API.Application.Services;
using To_Do_API.Domain.DTOs.TaskDTOs;
using To_Do_API.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using To_Do_API.Domain.Interfaces.TodoTasks;
using To_Do_API.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly TaskQueueHandler _taskQueueHandler;
        private readonly IHubContext<TasksHub> _taskCreationHub;

        public TasksController(ITaskService taskService, TaskQueueHandler taskQueueHandler, IHubContext<TasksHub> taskCreationHub)
        {
            _taskService = taskService;
            _taskQueueHandler = taskQueueHandler;
            _taskCreationHub  = taskCreationHub;
        }

        //GET: api/tasks
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();

            var pendingTasks = tasks.Where(t => t.Status == "Pending").ToList();

            var response = tasks.Select(t => new TaskResponseDto
            {
                Id = t.Id,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                Data = t.Data
            });
            return Ok(response);
        }

        //Get: api/tasks/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Data = task.Data
            };

            return Ok(response);
        }

        //Get: api/tasks/completionrate
        [Authorize]
        [HttpGet("completionrate")]
        public async Task<ActionResult<double>> GetCompletionRate()
        {
            var tasks = await _taskService.GetAllAsync();

            if (!tasks.Any())
                return BadRequest(new { error = "No hay tareas disponibles para calcular la tasa de finalización." });

            var completedTasks = tasks.Count(t => t.Status == "Completed");
            var totalTasks = tasks.ToList().Count;
            return  MemoizationHelper.CalculateCompletionPercentage(completedTasks, totalTasks);

        }

        //POST: api/tasks
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
        {
            try
            {

                var task = new TodoTask<string>
                {
                    Description = dto.Description,
                    DueDate = dto.DueDate,
                    Status = dto.Status,
                    Data = dto.Data
                };

                // Validate the task
                if(!TaskDelegates.validate(task))
                    return BadRequest(new {error = "La tarea no cumple con los parametros de validacion: La descripcion no puede estar vacia y/o la fecha de vencimiento no debe ser pasado." });

                //Enqueue the task for processing
                _taskQueueHandler.Enqueue(task);

                var response = new TaskResponseDto
                {
                    Id = task.Id,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    Data = task.Data
                };


                await _taskCreationHub.Clients.All.SendAsync("TaskCreated", response);

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //PUT: api/tasks/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTaskDto dto)
        {
            var existing = await _taskService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Description = dto.Description;
            existing.DueDate = dto.DueDate;
            existing.Status = dto.Status;
            existing.Data = dto.Data;

            var updated = await _taskService.UpdateAsync(existing);
            if (!updated)
                return BadRequest();

            return NoContent();

        }

        //DELETE: api/tasks/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpPost("customizable-task")]
        public async Task<ActionResult<TaskResponseDto>> CreateCustomTask([FromBody] string description, DateTime dueDate, string status, string data)
        {
            var task = TasksFactory.CreatePersonalizableTask(description, dueDate, status, data);

            //Enqueue the task for processing
            _taskQueueHandler.Enqueue(task);


            var response = new TaskResponseDto
            {
                Id = task.Id,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Data = task.Data,
            };
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [Authorize]
        [HttpPost("high-priority")]
        public async Task<ActionResult<TaskResponseDto>> CreateHighPriority([FromBody] string description) 
        { 
            var task = TasksFactory.CreateHighPriorityTask(description);

            //Enqueue the task for processing
            _taskQueueHandler.Enqueue(task);

            var response = new TaskResponseDto
            {
                Id = task.Id,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Data = task.Data,
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);

        }

        [Authorize]
        [HttpPost("low-priority")]
        public async Task<ActionResult<TaskResponseDto>> CreateLowPriority([FromBody] string description)
        {
            var task = TasksFactory.CreateLowPriorityTask(description);

            //Enqueue the task for processing
            _taskQueueHandler.Enqueue(task);
            var response = new TaskResponseDto
            {
                Id = task.Id,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                Data = task.Data,
            };

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);

        }
    }
}
