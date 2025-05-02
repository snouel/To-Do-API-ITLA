using Microsoft.AspNetCore.Mvc;
using To_Do_API.Models;
using To_Do_API.Repositories;
using To_Do_API.Services;

namespace To_Do_API.Controllers
{
    public class TasksController : ControllerBase
    {
        private readonly ITaskService<string> _taskService;

        public TasksController(ITaskService<string> taskService)
        {
            _taskService = taskService;
        }

        //GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTask<string>>>> GetAll() 
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        //Get: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask<string>>> GetById(Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        //POST: api/tasks
        //[HttpPost]
        //public async Task<ActionResult<TodoTask<string>>> Create(TodoTask<string> task) 
        //{

        //}
    }
}
