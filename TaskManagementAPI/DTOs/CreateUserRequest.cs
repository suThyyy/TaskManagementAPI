
using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class CreateUserRequest
{
    [Required]
    [MaxLength(100)]
    public string Fullname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
}
