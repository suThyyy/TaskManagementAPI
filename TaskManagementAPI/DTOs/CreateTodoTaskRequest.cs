
using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateTodoTaskRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }
}
