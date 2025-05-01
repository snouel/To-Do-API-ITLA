

using To_Do_API.Models;

namespace To_Do_API.Repositories
{
    public class TaskRepository<T> : ITaskRepository<T>
    {
        private readonly List<TodoTask<T>> _tasks = new();

        public async Task CreateAsync(TodoTask<T> task)
        {
            _tasks.Add(task);
            await Task.CompletedTask;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task is null) return await Task.FromResult(false);

            _tasks.Remove(task);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<TodoTask<T>>> GetAllAsync()
        {
            return await Task.FromResult(_tasks);
        }

        public async Task<TodoTask<T>?> GetByIdAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id); 
            return await Task.FromResult(task);
        }

        public async Task<bool> UpdateAsync(TodoTask<T> task)
        {
            var index = _tasks.FindIndex(t => t.Id == task.Id);

            if (index == -1) return await Task.FromResult(false);

            _tasks[index] = task;

            return await Task.FromResult(true);
        }
    }
}
