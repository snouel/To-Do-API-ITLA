using To_Do_API.Models;
using To_Do_API.Repositories;

namespace To_Do_API.Services
{
    public class TaskService<T> : ITaskService<T>
    {
        private readonly ITaskRepository<T> _repository;

        public TaskService(ITaskRepository<T> repository)
        {
            _repository = repository; 
        }
        public async Task<TodoTask<T>> CreateAsync(TodoTask<T> task)
        {
            if (string.IsNullOrWhiteSpace(task.Description))
                throw new ArgumentException("La descripcion no puede estar vacia.");

            if (task.DueDate <= DateTime.UtcNow)
                throw new ArgumentException("La fecha de vencimiento no debe ser futura.");

            task.Id = Guid.NewGuid();

            await _repository.CreateAsync(task);
            return task;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<TodoTask<T>>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TodoTask<T>?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(TodoTask<T> task)
        {
            return await _repository.UpdateAsync(task);
        }
    }
}
