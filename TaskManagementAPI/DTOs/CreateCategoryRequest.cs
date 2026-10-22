
using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateCategoryRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Description { get; set; }
}
