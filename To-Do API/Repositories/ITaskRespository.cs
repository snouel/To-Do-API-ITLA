using To_Do_API.Models;

namespace To_Do_API.Repositories
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
