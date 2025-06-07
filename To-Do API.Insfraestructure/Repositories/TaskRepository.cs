

using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.TodoTasks;

namespace To_Do_API.Infraestructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<TodoTask<string>> _tasks = new();
        private int _nextId = 1;

        public TaskRepository()
        {
            // Datos precargados
            _tasks.AddRange(new[]
            {
                new TodoTask<string>
                {
                    Id = 1,
                    Description = "Tarea 1",
                    DueDate = DateTime.UtcNow.AddDays(2),
                    Status = "Pending",
                    Data = "Estudio"
                },
                new TodoTask<string>
                {
                    Id = 2,
                    Description = "Tarea 2",
                    DueDate = DateTime.UtcNow.AddDays(3),
                    Status = "Completed",
                    Data = "Trabajo"
                }
            });
            // Establecer el siguiente ID evitando conflicto
            _nextId = _tasks.Max(t => t.Id) + 1;
        }

        public async Task CreateAsync(TodoTask<string> task)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            await Task.CompletedTask;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task is null) return await Task.FromResult(false);

            _tasks.Remove(task);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<TodoTask<string>>> GetAllAsync()
        {
            return await Task.FromResult(_tasks);
        }

        public async Task<TodoTask<string>?> GetByIdAsync(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id); 
            return await Task.FromResult(task);
        }

        public async Task<bool> UpdateAsync(TodoTask<string> task)
        {
            var index = _tasks.FindIndex(t => t.Id == task.Id);

            if (index == -1) return await Task.FromResult(false);

            _tasks[index] = task;

            return await Task.FromResult(true);
        }

      
    }
}
