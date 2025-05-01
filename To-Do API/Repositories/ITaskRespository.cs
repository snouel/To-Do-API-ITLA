using To_Do_API.Models;

namespace To_Do_API.Repositories
{

    public interface ITaskRepository<T>
    {
        Task<IEnumerable<TodoTask<T>>> GetAllAsync();
        Task<TodoTask<T>?> GetByIdAsync(Guid id);
        Task CreateAsync(TodoTask<T> task);
        Task<bool> UpdateAsync(TodoTask<T> task);
        Task<bool> DeleteAsync(Guid id);
    }

}
