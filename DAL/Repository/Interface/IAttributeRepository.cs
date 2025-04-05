using APIs;

namespace APIs.Repository.Interface
{
    public interface IAttributeRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<IEnumerable<Brand>> GetAllBrandsAsync();

        Task<IEnumerable<Style>> GetAllStylesAsync();

        Task<IEnumerable<Power>> GetAllPowersAsync();
    }
}
