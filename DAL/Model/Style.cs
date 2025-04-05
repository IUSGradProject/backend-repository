using System;
using System.Collections.Generic;

namespace APIs;

public partial class Style
{
    public int StyleId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
