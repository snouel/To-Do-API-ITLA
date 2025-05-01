namespace To_Do_API.Models
{
    public class TodoTask<T>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public T? Data { get; set; }
    }
}
