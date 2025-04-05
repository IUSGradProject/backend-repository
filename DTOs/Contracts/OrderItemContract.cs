using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Contracts
{
    public class OrderItemContract
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Available { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
