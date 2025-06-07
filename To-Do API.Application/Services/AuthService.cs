using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using To_Do_API.Domain.DTOs.UserDTOs;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.Auth;
using To_Do_API.Domain.Interfaces.Users;


namespace To_Do_API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<string> AuthenticateAsync(LogUserDTO dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.Email);
            if (user is null || user.Password != dto.Password)
                return null;


            var claims = new[]
            {
                new Claim(ClaimTypes.Email, dto.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public Task<bool> RegisterAsync(RegisterUserDTO dto)
        {
            var existingUser = _userRepository.GetUserByIdAsync(dto.Email);
            if (existingUser.Result != null)
                return Task.FromResult(false);

            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password,
                Username = dto.Username,
                CreatedAt = DateTime.UtcNow,
            };

            return _userRepository.AddUserAsync(user);

        }
    }
}
