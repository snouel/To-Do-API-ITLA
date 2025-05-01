using To_Do_API.Models;

namespace To_Do_API.Services
{
    public interface ITaskService<T>
    {
        Task<IEnumerable<TodoTask<T>>> GetAllAsync();
        Task<TodoTask<T>?> GetByIdAsync(Guid id);
        Task<TodoTask<T>> CreateAsync(TodoTask<T> task);
        Task<bool> UpdateAsync(TodoTask<T> task);
        Task<bool> DeleteAsync(Guid id);
    }
}
