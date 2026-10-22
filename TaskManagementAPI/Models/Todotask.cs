using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class Todotask
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool Completed { get; set; }

    public DateOnly? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
