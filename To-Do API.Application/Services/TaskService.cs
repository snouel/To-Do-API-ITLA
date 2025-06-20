using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.TodoTasks;

namespace To_Do_API.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository; 
        }
        public async Task<TodoTask<string>> CreateAsync(TodoTask<string> task)
        {
            if (string.IsNullOrWhiteSpace(task.Description))
                throw new ArgumentException("La descripcion no puede estar vacia.");

            if (task.DueDate <= DateTime.UtcNow)
                throw new ArgumentException("La fecha de vencimiento no debe ser pasado o la fecha de hoy.");

            await _repository.CreateAsync(task);
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la tarea debe ser mayor que cero.");
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TodoTask<string>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoTask<string>?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la tarea debe ser mayor que cero.");
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(TodoTask<string> task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), "La tarea no puede ser nula.");

            if (task.Id <= 0)
                throw new ArgumentException("El ID de la tarea debe ser mayor que cero.");

                if (string.IsNullOrWhiteSpace(task.Description))
                throw new ArgumentException("La descripcion no puede estar vacia.");

            if (task.DueDate <= DateTime.UtcNow)
                throw new ArgumentException("La fecha de vencimiento no debe ser pasado o la fecha de hoy.");

            return await _repository.UpdateAsync(task);
        }
    }
}
