using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Todotask> Todotasks { get; set; } = new List<Todotask>();
}
