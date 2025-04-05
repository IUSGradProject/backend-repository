using System;
using System.Collections.Generic;

namespace APIs;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string LastName { get; set; } = null!;

    public DateTime? LastRequest { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Role Role { get; set; } = null!;
}
