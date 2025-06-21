
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using To_Do_API.Application.Services;
using To_Do_API.Controllers;
using To_Do_API.Domain.DTOs;
using To_Do_API.Domain.DTOs.TaskDTOs;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.TodoTasks;
using To_Do_API.Hubs;

namespace To_Do_API.Tests.Controllers
{
    public class TasksControllersTests
    {
        private readonly TasksController _taskController;
        private readonly Mock<ITaskService> _mockTaskService;
        private readonly Mock<IHubContext<TasksHub>> _mockHub;
        private readonly Mock<TaskQueueHandler> _mockTaskQueueHandler;
        private readonly Mock<IHubClients> _mockHubClients;
        private readonly Mock<IClientProxy> _mockClientProxy;

        public TasksControllersTests()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockHub = new Mock<IHubContext<TasksHub>>();
            _mockTaskQueueHandler = new Mock<TaskQueueHandler>(_mockTaskService.Object);
            _mockClientProxy = new Mock<IClientProxy>();
            _mockHubClients = new Mock<IHubClients>();
            _taskController = new TasksController(_mockTaskService.Object,_mockTaskQueueHandler.Object, _mockHub.Object);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsOkAndTaskResponse()
        {
            // Arrange

            var task = new TodoTask<string>
            {
                Id = 1,
                Description = "Tarea desde test",
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Pending",
                Data = "mock"
            };

            _mockTaskService
                .Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(task);


            // Act
            var result = await _taskController.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<TaskResponseDto>(okResult.Value);
            Assert.Equal("Tarea desde test", dto.Description);
            Assert.Equal("Pending", dto.Status);
            Assert.Equal(1, dto.Id);
        }
    }
}
