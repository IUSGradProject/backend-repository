using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Contracts.Product
{
    public class UpdateProductContract
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string? Description { get; set; }

        public double Price { get; set; }

        public int SoldItems { get; set; }

        public int Available { get; set; }

        public int CategoryId { get; set; }

        public int BrandId { get; set; }

        public int StyleId { get; set; }

        public int PowerId { get; set; }
    }
}
