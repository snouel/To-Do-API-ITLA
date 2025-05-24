using To_Do_API.Models;
using System.Collections.Concurrent;
using System.Reactive.Linq;

namespace To_Do_API.Services
{
    public class TaskQueueHandler
    {
        private readonly ConcurrentQueue<TodoTask<string>> _taskQueueHandler = new();
        private bool _isProcessing = false;

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
                        await Task.Delay(2000);

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
