using To_Do_API.Domain.DTOs.UserDTOs;

namespace To_Do_API.Domain.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(LogUserDTO dto);
        Task<bool> RegisterAsync(RegisterUserDTO dto);
    }
}

