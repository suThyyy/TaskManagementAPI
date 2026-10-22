
using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class UpdateTodoTaskStatusRequest
{
    [Required]
    public bool? Completed { get; set; }
}
