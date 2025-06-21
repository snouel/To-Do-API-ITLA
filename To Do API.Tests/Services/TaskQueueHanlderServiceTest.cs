
using Moq;
using To_Do_API.Application.Services;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Interfaces.TodoTasks;

namespace To_Do_API.Tests.Services
{
    public class TaskQueueHanlderServiceTest
    {
        private readonly TaskQueueHandler _taskQueueHandler;
        private readonly Mock<ITaskService> _mockService;

        public TaskQueueHanlderServiceTest()
        {
            _mockService = new Mock<ITaskService>();
            _taskQueueHandler = new TaskQueueHandler(_mockService.Object);
        }

        [Fact]
        public async Task Enqueue_ValidarTarea_ProcesarYMarcarCompletada() 
        {
            //Arrang
            var task = new TodoTask<string>
            {
                Id = 1,
                Description = "Test Task",
                DueDate = DateTime.UtcNow.AddDays(1),
                Status = "Pending",
                Data = "Test Data"
            };

            _mockService
                .Setup(s => s.CreateAsync(It.IsAny<TodoTask<string>>()))
                .ReturnsAsync(task);

            //Act
            _taskQueueHandler.Enqueue(task);
            await Task.Delay(100); 

            //Assert
            _mockService.Verify(s => s.CreateAsync(It.IsAny<TodoTask<string>>()), Times.Once);
            Assert.Equal("Completed", task.Status);

        }

        [Fact]
        public async Task ProcessQueue_WhenQueueIsEmpty_DoesNotCallCreate()
        {

            // Arrange & Act
            _taskQueueHandler.ProcessQueue();
            await Task.Delay(100);

            // Assert
            _mockService.Verify(s => s.CreateAsync(It.IsAny<TodoTask<string>>()), Times.Never);
        }
    }
}
