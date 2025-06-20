
using Moq;
using To_Do_API.Application.Services;
using To_Do_API.Domain.Interfaces.TodoTasks;

namespace To_Do_API.Tests.Services
{
    public class TaskQueueHanlderServiceTest
    {
        private readonly TaskQueueHandler _taskQueueHandler;
        private readonly Mock<ITaskRepository> _mockRepo;

        public TaskQueueHanlderServiceTest()
        {
            _mockRepo = new Mock<ITaskRepository>();
            _taskQueueHandler = new TaskQueueHandler(_mockRepo.Object);
        }
    }
}
