using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using To_Do_API.Application.Settings;
using To_Do_API.Domain.DTOs.UserDTOs;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.Auth;
using To_Do_API.Domain.Interfaces.Users;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace To_Do_API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;
        public AuthService(IConfiguration config, IUserRepository userRepository, IOptions<JwtSettings> options)
        {
            _config = config;
            _userRepository = userRepository;
            _jwtSettings = options.Value;
        }

        public async Task<string> AuthenticateAsync(LogUserDTO dto)
        {
            try 
            {
                var user = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (user is null || user.Password != dto.Password || dto.Password is null)
                    throw new UnauthorizedAccessException("Invalid email or password.");


                var claims = new[]
                {
                new Claim(ClaimTypes.Email, dto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                };


                var rawKey = _jwtSettings.Key;
                if (string.IsNullOrEmpty(rawKey))
                    throw new InvalidOperationException("JWT secret key is missing from configuration.");

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                    (
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            } 
            catch (Exception ex)
            {
                throw;
            }
           

        }

        public async Task<bool> RegisterAsync(RegisterUserDTO dto)
        {
            try 
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
                if (existingUser != null)
                    throw new ValidationException("Este email ya esta registrado");
                if(dto.Username is null || dto.Password is null || dto.Email is null)
                    throw new ValidationException("Los campos de nombre, contrasena o correo no pueden ser nulos");
                var user = new User
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    Username = dto.Username,
                    CreatedAt = DateTime.UtcNow,
                };

                return await _userRepository.AddUserAsync(user);
            } 
            catch (Exception ex) 
            {
                throw;
            }
          

        }
    }
}
