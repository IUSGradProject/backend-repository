using System;
using System.Collections.Generic;

namespace APIs;

public partial class Power
{
    public int PowerId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
