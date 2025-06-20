using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Moq;
using To_Do_API.Application.Services;
using To_Do_API.Application.Settings;
using To_Do_API.Domain.Interfaces.Users;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.DTOs.UserDTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace To_Do_API.Tests.Services
{
    public  class AuthServicesTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly AuthService _authService;

        public AuthServicesTests() 
        {
            _mockUserRepo = new Mock<IUserRepository>();
            var jwtSettings = new JwtSettings
            {
                Key = "clave_super_secreta_1234567891011",
                Issuer = "test_issuer",
                Audience = "test_audience"
            };

            var options = Options.Create(jwtSettings);

            var mockConfig = new Mock<IConfiguration>();

            _authService = new AuthService(mockConfig.Object, _mockUserRepo.Object, options);
        }

        [Fact]
        public async Task Login_ConCredencialesValidas_RetornaToken() 
        {
            //Arrange
          

            _mockUserRepo
                .Setup(_mockUserRepo => _mockUserRepo.GetUserByEmailAsync("steven@mail.com"))
                .ReturnsAsync(new User
                {
                    Email = "steven@mail.com",
                    Password = "estaesmicontrasena"
                });

            //Act
            var result = await _authService.AuthenticateAsync(new LogUserDTO
            {
                Email = "steven@mail.com",
                Password = "estaesmicontrasena"
            });

            //Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.True(result.Length > 10); // Valida que aunque se genere el token string este tenga contenido.
        }

        [Fact]
        public async Task Login_ConContrasenaNula_RetornaLanzamientDeExcepcion() 
        {
            _mockUserRepo
              .Setup(_mockUserRepo => _mockUserRepo.GetUserByEmailAsync("steven@mail.com"))
              .ReturnsAsync(new User
              {
                  Email = "steven@mail.com",
                  Password = "estaesmicontrasena"
              });


            //Act 
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                 _authService.AuthenticateAsync(new LogUserDTO
                 {
                     Email = "steven@mail.com",
                     Password = null //Dado que lo que se introdujo en el input de login es una contraseña nula este debe retornar una excepción.
                 })
            );  

            //Assert
            Assert.Equal("Invalid email or password.", exception.Message);
        }

        [Fact]
        public async Task Login_ConCredencialesInvalidas_RetornaLanzamientDeExcepcion()
        {
            //Arrange

            _mockUserRepo
                .Setup(_mockUserRepo => _mockUserRepo.GetUserByEmailAsync("steven@mail.com"))
                .ReturnsAsync(new User
                {
                    Email = "steven@mail.com",
                    Password = "estaesmicontrasena"
                });

            var userInput = new LogUserDTO
            {
                Email = "steven@mail.com",
                Password = "contrasenaerronea" //Dado que lo que se introdujo en el input de login es una contraseña incorrecta a la que esta en el repositorio este debe retornar una excepción.
            };

            //Act 
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                 _authService.AuthenticateAsync(userInput)
            );

            //Assert
            Assert.Equal("Invalid email or password.", exception.Message);
        }

        [Fact]
        public async Task Registrar_CredencialesNulas_RetornaLanzamientoDeExcepcion()
        {
            //Arrange
           

            var dto = new RegisterUserDTO
            {
                Username = "steven",
                Email = "steven@mail.com",
                Password = null 
            };

            _mockUserRepo
                .Setup(_mockUserRepo => _mockUserRepo.GetUserByEmailAsync(dto.Email))
                .ReturnsAsync((User)null);

            //Act 
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                 _authService.RegisterAsync(dto)
            );

            //Assert
            Assert.Equal("Los campos de nombre, contrasena o correo no pueden ser nulos", exception.Message);

        }

        [Fact]
        public async Task Registrar_CredencialesValidas_RetornaTrue()
        {
      

            var dto = new RegisterUserDTO
            {
                Username = "steven",
                Email = "steven@mail.com",
                Password = "123"
            };

  
            //Simula que retorne true independientemente de los datos que se le pasen al repositorio
            _mockUserRepo
                .Setup(r => r.AddUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            //Act 
            var result = await _authService.RegisterAsync(dto);

            //Assert
            Assert.True(result);

        }
    }
}
