

using Moq;
using To_Do_API.Application.Services;
using To_Do_API.Domain.DTOs;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.TodoTasks;

namespace To_Do_API.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _taskService = new TaskService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateAsync_ConTareaValida_RetornaTarea() 
        {
            // Arrange
            var task = new TodoTask<string>
          
            {
                Id = 1,
                Description = "Nueva tarea",
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Pending",
                Data = "Datos adicionales"
            };

            _mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<TodoTask<string>>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _taskService.CreateAsync(task);

            //Assert 
            Assert.NotNull(result);
            Assert.Equal(task.Description, result.Description);
            Assert.Equal(task.DueDate, result.DueDate);
            Assert.Equal(task.Status, result.Status);
            Assert.Equal(task.Data, result.Data);

        }

        [Fact]
        public async Task UpdateAsync_ConIdInvalido_RetornaExcepcion()
        {
            // Arrange
            var task = new TodoTask<string>

            {
                Id = 0,
                Description = "Nueva tarea",
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Pending",
                Data = "Datos adicionales"
            };

            _mockRepository
                .Setup(x => x.UpdateAsync(It.IsAny<TodoTask<string>>()))
                .ReturnsAsync(false);

            //Act
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _taskService.UpdateAsync(task));

            //Assert 
            Assert.Equal("El ID de la tarea debe ser mayor que cero.", exception.Message);

        }

    }
}
