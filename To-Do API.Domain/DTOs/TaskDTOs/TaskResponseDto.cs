﻿namespace To_Do_API.Domain.DTOs.TaskDTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
        public string? Data { get; set; }
    }
}
