using To_Do_API.Domain.Entities;

namespace To_Do_API.Domain.Interfaces.TodoTasks
{

    public interface ITaskRepository
    {
        Task<IEnumerable<TodoTask<string>>> GetAllAsync();
        Task<TodoTask<string>?> GetByIdAsync(int id);
        Task CreateAsync(TodoTask<string> task);
        Task<bool> UpdateAsync(TodoTask<string> task);
        Task<bool> DeleteAsync(int id);
    }

}
