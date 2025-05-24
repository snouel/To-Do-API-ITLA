using To_Do_API.Models;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using To_Do_API.Helpers;

namespace To_Do_API.Services
{
    public class TaskQueueHandler
    {
        private readonly ConcurrentQueue<TodoTask<string>> _taskQueueHandler = new();
        private bool _isProcessing = false;
        private readonly IServiceScopeFactory _scopeFactory;

        public TaskQueueHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public void Enqueue(TodoTask<string> task)
        {
            task.Status = "Pending";
            _taskQueueHandler.Enqueue(task);
            ProcessQueue();
        }

        public void ProcessQueue() 
        {
            if (_isProcessing || _taskQueueHandler.IsEmpty)
                return;
            
            _isProcessing = true;

            Observable
                .Start(async () =>
                {
                    if (_taskQueueHandler.TryDequeue(out var currentTask))
                    {
                        Console.WriteLine($"Procesando tarea: {currentTask.Description}");

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();
                            await taskService.CreateAsync(currentTask);

                            // Notify the creation
                            TaskDelegates.NotifyCreation(currentTask);

                            // Calculate the days left
                            var daysLeft = TaskDelegates.CalculateDayLeft(currentTask);
                            Console.WriteLine($"Dias restantes: {daysLeft}");
                        }

                        currentTask.Status = "Completed";
                        Console.WriteLine($"Completada: {currentTask.Description}");
                    }
                })
                .Finally(() =>
                {
                    _isProcessing = false;
                    ProcessQueue();
                })
                .Subscribe();
        }

    }
}
