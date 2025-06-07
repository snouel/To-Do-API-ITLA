
using To_Do_API.Domain.Entities;

namespace To_Do_API.Domain.DTOs.UserDTOs
{
    public class LogUserDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
