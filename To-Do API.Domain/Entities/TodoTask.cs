namespace To_Do_API.Domain.Entities
{
    public class TodoTask<T>
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public T? Data { get; set; }
    }
}
