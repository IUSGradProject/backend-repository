using APIs.Contracts.Product;

namespace APIs.Contracts
{
    public class PaginatedDataContract<T>
    {
        public PaginatedDataContract(T Data, int PageNumber, int PageSize, int Count)
        {
            this.Data = Data;
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.TotalCount = Count;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public T Data { get; set; }
}
}
