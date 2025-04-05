using System;
using System.Collections.Generic;

namespace APIs;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string? Description { get; set; }

    public double Price { get; set; }

    public int SoldItems { get; set; }

    public int Available { get; set; }

    public int CategoryId { get; set; }

    public bool IsEditing { get; set; }

    public int Version { get; set; }

    public bool IsLast { get; set; }

    public int BrandId { get; set; }

    public int StyleId { get; set; }

    public int PowerId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    public virtual Category Category { get; set; } = null!;

    public virtual Brand Brand { get; set; } = null!;

    public virtual Power Power { get; set; } = null!;

    public virtual Style Style { get; set; } = null!;
}
