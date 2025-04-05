using APIs.Contracts;
using APIs;

namespace BLL.Services.Interface
{
    public interface IAttributeService
    {
        Task<IEnumerable<LoV>> GetAllCategoriesAsync();

        Task<IEnumerable<LoV>> GetAllBrandsAsync();

        Task<IEnumerable<LoV>> GetAllStylesAsync();

        Task<IEnumerable<LoV>> GetAllPowersAsync();
    }
}
