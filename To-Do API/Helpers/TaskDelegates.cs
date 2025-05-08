using To_Do_API.Models;

namespace To_Do_API.Helpers
{
    public class TaskDelegates
    {
        public delegate bool ValidateTask(TodoTask<string> task);

        public static ValidateTask validate = task => !string.IsNullOrWhiteSpace(task.Description) && task.DueDate > DateTime.UtcNow;

        public static Action<TodoTask<string>> NotifyCreation = task =>
        {
            Console.WriteLine($"Task Created: {task.Description} with ID: {task.Id}");
        };

        public static Func<TodoTask<string>, int> CalculateDayLeft = task => (task.DueDate - DateTime.Now).Days;
    }
}
