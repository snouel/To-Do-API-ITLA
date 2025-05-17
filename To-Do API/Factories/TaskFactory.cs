using To_Do_API.Models;

namespace To_Do_API.Factories
{
    public class TasksFactory
    {

        public static TodoTask<string> CreatePersonalizableTask(string description, DateTime dueDate, string status, string? data)
        {
            return new TodoTask<string>
            {
                Description = description,
                DueDate = dueDate,
                Status = status,
                Data = data
            };
        }

        public static TodoTask<string> CreateHighPriorityTask(string description)
        {
            return new TodoTask<string>
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(1),
                Status = "Pending",
                Data = "High Priority"
            };
        }

        public static TodoTask<string> CreateLowPriorityTask(string description)
        {
            return new TodoTask<string>
            {
                Description = description,
                DueDate = DateTime.Now.AddDays(7),
                Status = "Pending",
                Data = "Low Priority"
            };
        }


    }
}
