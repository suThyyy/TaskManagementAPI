using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Todotask> Todotasks { get; set; } = new List<Todotask>();
}
