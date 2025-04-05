using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Contracts.Product
{
    public class ProductBaseContract
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
        public int SoldItems { get; set; }
        public int Available { get; set; }
    }
}
