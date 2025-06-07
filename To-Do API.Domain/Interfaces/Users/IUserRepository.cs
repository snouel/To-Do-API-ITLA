using To_Do_API.Domain.Entities;

namespace To_Do_API.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(string username);
    }
}
