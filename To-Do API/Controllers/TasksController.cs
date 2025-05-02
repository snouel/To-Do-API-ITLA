using Microsoft.AspNetCore.Mvc;
using To_Do_API.DTOs;
using To_Do_API.Models;
using To_Do_API.Repositories;
using To_Do_API.Services;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        //GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();

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

        //POST: api/tasks
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

                var created = await _taskService.CreateAsync(task);

                var response = new TaskResponseDto
                {
                    Id = created.Id,
                    Description = created.Description,
                    DueDate = created.DueDate,
                    Status = created.Status,
                    Data = created.Data
                };

                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        //PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateTaskDto dto) 
        {
            var existing = await _taskService.GetByIdAsync(id);
            if(existing == null)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var result = await _taskService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
