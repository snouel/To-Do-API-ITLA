﻿namespace To_Do_API.Domain.DTOs.UserDTOs
{
    public class RegisterUserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
