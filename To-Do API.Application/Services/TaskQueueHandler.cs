using System.Collections.Concurrent;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection;
using To_Do_API.Domain.Entities;
using To_Do_API.Domain.Delegates;
using To_Do_API.Domain.Interfaces.TodoTasks;




namespace To_Do_API.Application.Services
{
    public class TaskQueueHandler
    {
        private readonly ConcurrentQueue<TodoTask<string>> _taskQueueHandler = new();
        private bool _isProcessing = false;
        private readonly ITaskService _taskService;

        public TaskQueueHandler(ITaskService taskService)
        {
            _taskService = taskService;
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
                        await _taskService.CreateAsync(currentTask);

                            // Notify the creation
                            TaskDelegates.NotifyCreation(currentTask);

                            // Calculate the days left
                            var daysLeft = TaskDelegates.CalculateDayLeft(currentTask);
                            Console.WriteLine($"Dias restantes: {daysLeft}");
                        

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
