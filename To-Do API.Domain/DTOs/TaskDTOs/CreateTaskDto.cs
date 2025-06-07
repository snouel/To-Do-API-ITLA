namespace To_Do_API.Domain.DTOs
{
    public class CreateTaskDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Data { get; set; }
    }
}
