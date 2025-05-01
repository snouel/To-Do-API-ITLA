using Microsoft.AspNetCore.Mvc;
using To_Do_API.Repositories;

namespace To_Do_API.Controllers
{
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository<string> _repository;

        public TasksController()
        {
            _repository = new TaskRepository<string>();
        }



    }
}
