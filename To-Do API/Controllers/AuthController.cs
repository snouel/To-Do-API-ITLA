using Microsoft.AspNetCore.Mvc;
using To_Do_API.Application.Services;
using To_Do_API.Domain.DTOs.UserDTOs;
using To_Do_API.Domain.Interfaces.Auth;

namespace To_Do_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogUserDTO dto)
        {
            var token = await _authService.AuthenticateAsync(dto);
            if (token == null)
                return Unauthorized(new { message = "Credenciales invalidas" });

            return Ok(new { token });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result)
                return BadRequest(new { message = "El usuario ya existe" });
            return Ok(new { message = "Usuario registrado exitosamente" });
        }
    }
}
