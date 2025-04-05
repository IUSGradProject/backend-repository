using System;
using System.Collections.Generic;

namespace APIs;

public partial class MaterialProduct
{
    public int MaterialId { get; set; }

    public Guid ProductId { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
