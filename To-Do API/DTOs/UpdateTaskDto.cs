namespace To_Do_API.DTOs
{
    public class UpdateTaskDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Data { get; set; }
    }
}
