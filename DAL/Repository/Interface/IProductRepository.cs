using APIs;
using DTOs.Contracts;

namespace APIs.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(FilterRequest? filters);

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetSoldOutProductsAsync();

        Task<Product> GetProductByIdAsync(Guid productId);

        Task AddProductAsync(Product product);

        Task DeleteProductAsync(Guid productId);
        Task<List<Product>> GetAllDeletedProductsAsync();
        Task RestoreProductAsync(Guid productId);

        Task UpdateProductAsync(Product product);

        Task SaveCart(User user);

        Task OrderProduct(Guid productId, int quantity);

        Task<Product> GetProductForEdit(Guid productId);
    }
}
