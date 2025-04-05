using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIs.Contracts;

namespace DTOs.Contracts
{
    public class FilterRequest
    {
        public List<int> Categories { get; set; } = new List<int>();
        public List<int> Styles { get; set; } = new List<int>();
        public List<int> Brands { get; set; } = new List<int>();
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public string? Query { get; set; }
        public int? QueryCategoryId { get; set; }
        public string? SortBy { get; set; }
        public bool SortDesc { get; set; }
    }
}
