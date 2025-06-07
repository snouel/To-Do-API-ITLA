using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.Users;

namespace To_Do_API.Insfraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly ConcurrentDictionary<string, User> _users = new();
        private static int _currentId = 0;

        public Task<bool> AddUserAsync(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Username))  
            {
                return Task.FromResult(false);
            }

            user.Id = Interlocked.Increment(ref _currentId);    
            var result = _users.TryAdd(user.Email, user);
            return Task.FromResult(result);
        }

        public Task<User?> GetUserByIdAsync(string email)
        {
            _users.TryGetValue(email, out var user);
            return Task.FromResult(user);
        }
    }
}
