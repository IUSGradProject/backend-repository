﻿using System;
using System.Collections.Generic;

namespace APIs;

public partial class CartProduct
{
    public Guid CartProductId { get; set; }

    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
