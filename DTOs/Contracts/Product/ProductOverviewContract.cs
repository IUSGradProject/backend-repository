namespace APIs.Contracts.Product
{
    public class ProductOverviewContract
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string Image { get; set; } = null!;

        public string? Description { get; set; }

        public double Price { get; set; }

        public int SoldItems { get; set; }

        public int Available { get; set; }

        public bool IsEditing { get; set; }

        public LoV Category { get; set; } = null!;

        public LoV Brand { get; set; } = null!;

        public LoV Power { get; set; } = null!;

        public LoV Style { get; set; } = null!;

        public IEnumerable<LoV> Materials { get; set; } = new List<LoV>();

    }
}
