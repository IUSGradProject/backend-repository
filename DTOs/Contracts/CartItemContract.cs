namespace DTOs.Contracts
{
    public class CartItemContract
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Available { get; set; }

        public int Quantity { get; set; }
    }
}