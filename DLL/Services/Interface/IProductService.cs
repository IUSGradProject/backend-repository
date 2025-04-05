using APIs;
using APIs.Contracts;
using APIs.Contracts.Product;
using DTOs.Contracts;
using DTOs.Contracts.Product;

namespace BLL.Services.Interface
{
    public interface IProductService
    {
        Task<PaginatedDataContract<IEnumerable<ProductContract>>> GetProductsAsync(FilterRequest filters, int pageNumber = 1, int pageSize = 20);

        Task<PaginatedDataContract<IEnumerable<ProductBaseContract>>> GetAllProductsAsync(int pageNumber = 1, int pageSize = 20);
        Task<ProductOverviewContract> GetProductAsync(Guid productId);
        Task<IEnumerable<Product>> GetSoldOutProductsAsync();
        Task AddProductAsync(UpdateProductContract createProduct);
        Task UpdateProductAsync(UpdateProductContract product);
        Task<UpdateProductContract> GetProductForEdit(Guid productId);
        Task DeleteAsync(Guid id);
        Task<List<Product>> GetAllDeletedProductsAsync();
        Task RestoreAsync(Guid id);
    }
}
