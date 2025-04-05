namespace APIs.Contracts.Product
{
    public class ProductContract
    {
        public ProductContract()
        {
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public int Available { get; set; }
        public int Brand { get; set;  }
        public int Style { get; set; }
    }

}
