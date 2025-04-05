using System;
using System.Collections.Generic;

namespace APIs;

public partial class Cart
{
    public Guid UserId { get; set; }

    public int Paid { get; set; }

    public Guid CartId { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    public virtual User User { get; set; } = null!;
}
