using APIs;

namespace APIs.Repository.Interface
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetMaterialsByProductIdAsync(Guid productId);
    }
}
